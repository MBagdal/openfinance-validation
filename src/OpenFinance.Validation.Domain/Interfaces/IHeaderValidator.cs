using Microsoft.AspNetCore.Http;

namespace OpenFinance.Validation.Domain.Interfaces;

/// <summary>
/// Interface para validação de headers
/// </summary>
public interface IHeaderValidator
{
    /// <summary>
    /// Valida headers obrigatórios para requisições POST
    /// </summary>
    bool ValidateIdempotencyHeaders(HttpRequest request);
    
    /// <summary>
    /// Valida headers obrigatórios para requisições GET
    /// </summary>
    bool ValidateNonIdempotencyHeaders(HttpRequest request);
}