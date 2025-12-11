using System.Security.Cryptography.X509Certificates;
using OpenFinance.Validation.Domain.Constants;
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
    /// <param name="tokenDetails">Detalhes do token</param>
    /// <param name="clientCert">Certificado do cliente</param>
    /// <param name="requiredScope">Escopo requerido (padrão: payments)</param>
    /// <returns>True se válido, False caso contrário</returns>
    public bool Validate(TokenDetails tokenDetails, X509Certificate2? clientCert, string requiredScope = OAuth2Scopes.Resources)
    {
        if (!tokenDetails.Active)
            return false;

        if (!tokenDetails.HasScope(requiredScope))
            return false;

        if (clientCert == null)
            return false;

        if (tokenDetails.Cnf == null || !tokenDetails.Cnf.ContainsKey("x5t#S256"))
            return false;

        var certThumbprint = _certificateService.GetCertThumbprint(clientCert);
        var tokenThumbprint = tokenDetails.Cnf.GetValueOrDefault("x5t#S256");

        return certThumbprint == tokenThumbprint;
    }
}