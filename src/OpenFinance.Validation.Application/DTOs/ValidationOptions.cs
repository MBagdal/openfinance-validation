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
}