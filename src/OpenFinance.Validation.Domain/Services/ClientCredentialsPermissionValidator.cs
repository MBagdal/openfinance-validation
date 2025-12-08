using System.Security.Cryptography.X509Certificates;
using OpenFinance.Validation.Domain.Interfaces;
using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Domain.Services;

/// <summary>
/// Validador de permissões de client credentials
/// </summary>
public class ClientCredentialsPermissionValidator
{
    private readonly ICertificateService _certificateService;

    public ClientCredentialsPermissionValidator(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    /// <summary>
    /// Valida as permissões do client credentials
    /// </summary>
    public bool Validate(TokenDetails tokenDetails, X509Certificate2? clientCert, string scope)
    {
        if (!tokenDetails.Active)
            return false;

        if (!tokenDetails.HasScope(scope))
            return false;

        if (clientCert == null)
            return false;

        var certThumbprint = _certificateService.GetCertThumbprint(clientCert);
        var tokenThumbprint = tokenDetails.Cnf.GetValueOrDefault("x5t#S256");

        return certThumbprint == tokenThumbprint;
    }
}