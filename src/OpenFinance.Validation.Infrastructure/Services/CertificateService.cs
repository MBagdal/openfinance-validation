using Microsoft.AspNetCore.Http;
using OpenFinance.Validation.Domain.Interfaces;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OpenFinance.Validation.Infrastructure.Services;

/// <summary>
/// Implementação do serviço de certificados
/// </summary>
public class CertificateService : ICertificateService
{
    public X509Certificate2? GetClientCertificate(HttpRequest request)
    {
        return request.HttpContext.Connection.ClientCertificate;
    }

    public string GetCertThumbprint(X509Certificate2 certificate)
    {
        var thumbprint = certificate.GetCertHashString();
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(thumbprint));
        return Convert.ToBase64String(hash);
    }
}