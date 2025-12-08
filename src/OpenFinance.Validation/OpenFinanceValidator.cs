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
        _authValidator = authValidator;
        _headerValidator = headerValidator;
        _signatureValidator = signatureValidator;
        _audienceService = audienceService;
        _responseService = responseService;
        _logger = logger;
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
        var tokenDetails = await _authValidator.ValidateAuthenticationAsync(request);
        if (tokenDetails == null)
        {
            throw new UnauthorisedException(_responseService.ReturnUnauthorized());
        }

        var headersValid = options.IsIdempotencyValidationNecessary
            ? _headerValidator.ValidatePostHeaders(request)
            : _headerValidator.ValidateGetHeaders(request);

        if (!headersValid)
        {
            throw new BadRequestException(_responseService.ReturnBadRequest());
        }

        string? audience = null;
        if (options.IsAudienceValidationNecessary)
        {
            audience = _audienceService.GetAudience(request.Path, options.AudienceParams);
        }

        object? payload = null;
        string? clientOrganisationId = null;
        
        if (options.IsPayloadExtractionNecessary)
        {
            var (extractedPayload, extractedOrgId) = await _signatureValidator.ValidateRequestSignatureAsync(
                request, 
                tokenDetails.ClientId, 
                audience);
            
            if (extractedPayload == null)
            {
                throw new BadSignatureException(_responseService.ReturnBadSignature());
            }
            
            payload = extractedPayload;
            clientOrganisationId = extractedOrgId;
        }

        return ValidationResult.SuccessResult(payload, clientOrganisationId, tokenDetails);
    }
}