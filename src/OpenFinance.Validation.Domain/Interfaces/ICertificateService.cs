using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;

namespace OpenFinance.Validation.Domain.Interfaces;

/// <summary>
/// Interface para serviços de certificado
/// </summary>
public interface ICertificateService
{
    /// <summary>
    /// Obtém o certificado do cliente da requisição
    /// </summary>
    X509Certificate2? GetClientCertificate(HttpRequest request);
    
    /// <summary>
    /// Obtém o thumbprint do certificado
    /// </summary>
    string GetCertThumbprint(X509Certificate2 certificate);
}