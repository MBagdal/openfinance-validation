using Microsoft.AspNetCore.Http;
using OpenFinance.Validation.Domain.Constants;

namespace OpenFinance.Validation.Application.DTOs;

/// <summary>
/// Opções de validação para requisições Open Finance
/// </summary>
public class ValidationOptions
{
    /// <summary>
    /// Se deve validar o audience do JWT
    /// </summary>
    public bool IsAudienceValidationNecessary { get; set; }
    
    /// <summary>
    /// Parâmetros adicionais para construção do audience
    /// </summary>
    public string? AudienceParams { get; set; }
    
    /// <summary>
    /// Se deve validar header de idempotência (POST e PATCH requests)
    /// </summary>
    public bool IsIdempotencyValidationNecessary { get; set; }
    
    /// <summary>
    /// Se deve extrair e validar o payload assinado
    /// </summary>
    public bool IsPayloadExtractionNecessary { get; set; }

    /// <summary>
    /// Auto-configura todas as opções baseado no método HTTP da requisição
    /// </summary>
    /// <param name="request">HttpRequest para análise</param>
    public void AutoConfigureFromRequest(HttpRequest request)
    {
        var method = request.Method.ToUpperInvariant();
        
        IsIdempotencyValidationNecessary = HttpMethodHelper.RequiresIdempotencyValidation(request.Method);
        IsPayloadExtractionNecessary = HttpMethodHelper.HaveRequestBody(method);
        IsAudienceValidationNecessary = true;
    }
}