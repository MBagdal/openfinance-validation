using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenFinance.Validation.Domain.Interfaces;
using OpenFinance.Validation.Domain.ValueObjects;
using OpenFinance.Validation.Domain.Exceptions;
using OpenFinance.Validation.Application.Interfaces;
using OpenFinance.Validation.Application.DTOs;

namespace OpenFinance.Validation;

/// <summary>
/// Validador principal para requisições Open Finance Brasil
/// </summary>
public class OpenFinanceValidator
{
    private readonly IAuthenticationValidator _authValidator;
    private readonly IHeaderValidator _headerValidator;
    private readonly IRequestSignatureValidator _signatureValidator;
    private readonly IAudienceService _audienceService;
    private readonly IResponseService _responseService;
    private readonly ILogger<OpenFinanceValidator> _logger;

    public OpenFinanceValidator(
        IAuthenticationValidator authValidator,
        IHeaderValidator headerValidator,
        IRequestSignatureValidator signatureValidator,
        IAudienceService audienceService,
        IResponseService responseService,
        ILogger<OpenFinanceValidator> logger)
    {
        _authValidator = authValidator ?? throw new ArgumentNullException(nameof(authValidator));
        _headerValidator = headerValidator ?? throw new ArgumentNullException(nameof(headerValidator));
        _signatureValidator = signatureValidator ?? throw new ArgumentNullException(nameof(signatureValidator));
        _audienceService = audienceService ?? throw new ArgumentNullException(nameof(audienceService));
        _responseService = responseService ?? throw new ArgumentNullException(nameof(responseService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Valida uma requisição Open Finance completa com auto-configuração
    /// </summary>
    /// <param name="request">HttpRequest da requisição</param>
    /// <returns>Resultado da validação</returns>
    /// <exception cref="UnauthorisedException">Token inválido ou ausente</exception>
    /// <exception cref="BadRequestException">Headers obrigatórios ausentes</exception>
    /// <exception cref="BadSignatureException">Assinatura JWT inválida</exception>
    public async Task<ValidationResult> ValidateRequestAsync(HttpRequest request)
    {
        var options = new ValidationOptions();
        options.AutoConfigureFromRequest(request);

        return await ValidateRequestAsync(request, options);
    }

    /// <summary>
    /// Valida uma requisição Open Finance completa
    /// </summary>
    /// <param name="request">HttpRequest da requisição</param>
    /// <param name="options">Opções de validação</param>
    /// <returns>Resultado da validação</returns>
    /// <exception cref="UnauthorisedException">Token inválido ou ausente</exception>
    /// <exception cref="BadRequestException">Headers obrigatórios ausentes</exception>
    /// <exception cref="BadSignatureException">Assinatura JWT inválida</exception>
    public async Task<ValidationResult> ValidateRequestAsync(
        HttpRequest request,
        ValidationOptions options)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        _logger.LogDebug(
            "Starting validation for {Method} {Path}. Idempotency: {Idempotency}, Payload: {Payload}, Audience: {Audience}",
            request.Method,
            request.Path,
            options.IsIdempotencyValidationNecessary,
            options.IsPayloadExtractionNecessary,
            options.IsAudienceValidationNecessary);

        var tokenDetails = await ValidateAuthenticationAsync(request);

        ValidateHeaders(request, options);

        var audience = GetAudience(request, options);
        var (payload, clientOrganisationId) = await ValidateSignatureAndExtractPayloadAsync(
            request, 
            tokenDetails, 
            audience, 
            options);

        _logger.LogDebug(
            "Validation completed successfully for {Method} {Path}. ClientOrgId: {ClientOrgId}",
            request.Method,
            request.Path,
            clientOrganisationId);

        return ValidationResult.SuccessResult(payload, clientOrganisationId, tokenDetails);
    }

    /// <summary>
    /// Valida a autenticação da requisição
    /// </summary>
    private async Task<TokenDetails> ValidateAuthenticationAsync(HttpRequest request)
    {
        _logger.LogDebug("Validating authentication for {Path}", request.Path);

        var tokenDetails = await _authValidator.ValidateAuthenticationAsync(request);
        
        if (tokenDetails == null)
        {
            _logger.LogWarning("Authentication failed for {Path}. Token is invalid or missing.", request.Path);
            throw new UnauthorisedException(_responseService.ReturnUnauthorized());
        }

        _logger.LogDebug("Authentication successful. ClientId: {ClientId}", tokenDetails.ClientId);
        return tokenDetails;
    }

    /// <summary>
    /// Valida os headers obrigatórios da requisição
    /// </summary>
    private void ValidateHeaders(HttpRequest request, ValidationOptions options)
    {
        _logger.LogDebug(
            "Validating headers for {Path}. Idempotency required: {Idempotency}",
            request.Path,
            options.IsIdempotencyValidationNecessary);

        var headersValid = options.IsIdempotencyValidationNecessary
            ? _headerValidator.ValidateIdempotencyHeaders(request)
            : _headerValidator.ValidateNonIdempotencyHeaders(request);

        if (!headersValid)
        {
            _logger.LogWarning(
                "Header validation failed for {Method} {Path}. Required headers missing.",
                request.Method,
                request.Path);
            throw new BadRequestException(_responseService.ReturnBadRequest());
        }

        _logger.LogDebug("Header validation successful for {Path}", request.Path);
    }

    /// <summary>
    /// Obtém o audience se necessário
    /// </summary>
    private string? GetAudience(HttpRequest request, ValidationOptions options)
    {
        if (!options.IsAudienceValidationNecessary)
        {
            _logger.LogDebug("Audience validation skipped for {Path}", request.Path);
            return null;
        }

        _logger.LogDebug("Getting audience for {Path}", request.Path);
        var audience = _audienceService.GetAudience(request.Path, options.AudienceParams);
        _logger.LogDebug("Audience obtained: {Audience}", audience);
        return audience;
    }

    /// <summary>
    /// Valida a assinatura e extrai o payload se necessário
    /// </summary>
    private async Task<(object? Payload, string? ClientOrganisationId)> ValidateSignatureAndExtractPayloadAsync(
        HttpRequest request,
        TokenDetails tokenDetails,
        string? audience,
        ValidationOptions options)
    {
        if (!options.IsPayloadExtractionNecessary)
        {
            _logger.LogDebug("Payload extraction skipped for {Path}", request.Path);
            return (null, null);
        }

        _logger.LogDebug(
            "Validating signature and extracting payload for {Path}. ClientId: {ClientId}",
            request.Path,
            tokenDetails.ClientId);

        var (extractedPayload, extractedOrgId) = await _signatureValidator.ValidateRequestSignatureAsync(
            request,
            tokenDetails.ClientId,
            audience);

        if (extractedPayload == null)
        {
            _logger.LogWarning(
                "Signature validation failed for {Path}. ClientId: {ClientId}",
                request.Path,
                tokenDetails.ClientId);
            throw new BadSignatureException(_responseService.ReturnBadSignature());
        }

        _logger.LogDebug(
            "Signature validation successful. ClientOrgId: {ClientOrgId}",
            extractedOrgId);

        return (extractedPayload, extractedOrgId);
    }
}