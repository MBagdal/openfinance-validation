using Microsoft.AspNetCore.Http;

namespace OpenFinance.Validation.Domain.Interfaces;

/// <summary>
/// Interface para validação de assinatura de requisição
/// </summary>
public interface IRequestSignatureValidator
{
    /// <summary>
    /// Valida a assinatura JWT da requisição
    /// </summary>
    Task<(object? Payload, string? ClientOrganisationId)> ValidateRequestSignatureAsync(
        HttpRequest request, 
        string clientId, 
        string? audience);
}