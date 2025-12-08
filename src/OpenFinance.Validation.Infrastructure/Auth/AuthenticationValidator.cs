using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenFinance.Validation.Domain.Interfaces;
using OpenFinance.Validation.Domain.ValueObjects;
using OpenFinance.Validation.Domain.Services;

namespace OpenFinance.Validation.Infrastructure.Auth;

/// <summary>
/// Implementação do validador de autenticação
/// </summary>
public class AuthenticationValidator : IAuthenticationValidator
{
    private readonly IAuthorizationServerClient _authServerClient;
    private readonly ICertificateService _certificateService;
    private readonly ClientCredentialsPermissionValidator _permissionValidator;
    private readonly ILogger<AuthenticationValidator> _logger;

    public AuthenticationValidator(
        IAuthorizationServerClient authServerClient,
        ICertificateService certificateService,
        ClientCredentialsPermissionValidator permissionValidator,
        ILogger<AuthenticationValidator> logger)
    {
        _authServerClient = authServerClient;
        _certificateService = certificateService;
        _permissionValidator = permissionValidator;
        _logger = logger;
    }

    public async Task<TokenDetails?> ValidateAuthenticationAsync(HttpRequest request)
    {
        var authHeader = request.Headers["Authorisation"].ToString();
        if (string.IsNullOrEmpty(authHeader))
        {
            _logger.LogWarning("Authorization header is missing");
            return null;
        }

        var tokenDetails = await _authServerClient.IntrospectAccessTokenAsync(authHeader);
        if (tokenDetails == null)
        {
            _logger.LogWarning("Token introspection failed");
            return null;
        }

        var clientCert = _certificateService.GetClientCertificate(request);
        if (!_permissionValidator.Validate(tokenDetails, clientCert))
        {
            _logger.LogWarning("Client credentials validation failed");
            return null;
        }

        return tokenDetails;
    }
}