using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using OpenFinance.Validation.Domain.Interfaces;

namespace OpenFinance.Validation.Infrastructure.Jwt;

/// <summary>
/// Implementação do validador de assinatura JWT
/// </summary>
public class JwtSignatureValidator : IRequestSignatureValidator
{
    private readonly IAuthorizationServerClient _authServerClient;
    private readonly IDirectoryClient _directoryClient;
    private readonly IUtilsService _utilsService;
    private readonly ILogger<JwtSignatureValidator> _logger;

    public JwtSignatureValidator(
        IAuthorizationServerClient authServerClient,
        IDirectoryClient directoryClient,
        IUtilsService utilsService,
        ILogger<JwtSignatureValidator> logger)
    {
        _authServerClient = authServerClient;
        _directoryClient = directoryClient;
        _utilsService = utilsService;
        _logger = logger;
    }

    public async Task<(object? Payload, string? ClientOrganisationId)> ValidateRequestSignatureAsync(
        HttpRequest request,
        string clientId,
        string? audience)
    {
        try
        {
            var client = await _authServerClient.GetClientDetailsAsync(clientId);
            var clientJwks = await _directoryClient.GetClientKeysAsync(client);
            var clientOrganisationId = _utilsService.ExtractOrgIdFromJwksUri(client.JwksUri);

            // Alternativa sem EnableBuffering - ler o body diretamente
            string signedRequestBody;
            using (var reader = new StreamReader(request.Body, leaveOpen: true))
            {
                signedRequestBody = await reader.ReadToEndAsync();
            }

            var payload = await ValidateSignedRequestAsync(
                clientJwks,
                clientOrganisationId,
                signedRequestBody,
                audience);

            return (payload, clientOrganisationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao tentar validar os dados da requisição");
            return (null, null);
        }
    }

    private async Task<object?> ValidateSignedRequestAsync(
        JsonWebKeySet clientJwks,
        string clientOrganisationId,
        string signedRequestBody,
        string? audience)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = clientOrganisationId,
                ValidateAudience = !string.IsNullOrEmpty(audience),
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(5),
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                IssuerSigningKeys = clientJwks.GetSigningKeys()
            };

            var principal = handler.ValidateToken(signedRequestBody, validationParameters, out var validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;

            return jwtToken?.Payload;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao tentar validar a assinatura da requisição");
            return null;
        }
    }
}