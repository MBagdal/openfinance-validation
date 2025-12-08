namespace OpenFinance.Validation.Domain.ValueObjects;

/// <summary>
/// Resultado da validação de uma requisição
/// </summary>
public class ValidationResult
{
    public bool Success { get; init; }
    public object? Payload { get; init; }
    public string? ClientOrganisationId { get; init; }
    public TokenDetails? TokenDetails { get; init; }
    public static ValidationResult SuccessResult(object? payload, string? clientOrgId, TokenDetails? tokenDetails) 
        => new() { Success = true, Payload = payload, ClientOrganisationId = clientOrgId, TokenDetails = tokenDetails };
}