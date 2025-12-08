using Microsoft.AspNetCore.Http;
using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Domain.Interfaces;

/// <summary>
/// Interface para validação de autenticação
/// </summary>
public interface IAuthenticationValidator
{
    /// <summary>
    /// Valida o token de autenticação da requisição
    /// </summary>
    Task<TokenDetails?> ValidateAuthenticationAsync(HttpRequest request);
}