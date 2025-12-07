namespace OpenFinance.Validation.Domain.ValueObjects;

/// <summary>
/// Resposta de erro padronizada Open Finance
/// </summary>
public class ErrorResponse
{
    public List<ErrorDetail> Errors { get; init; } = new();
    public ResponseMeta Meta { get; init; } = new();
}

/// <summary>
/// Detalhe de um erro
/// </summary>
public class ErrorDetail
{
    public string Code { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;
}

/// <summary>
/// Metadados da resposta
/// </summary>
public class ResponseMeta
{
    public DateTime RequestDateTime { get; init; } = DateTime.UtcNow;
    
    public string RequestDateTimeIso => RequestDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

    public string RequestDateTimeMonetaryIso => RequestDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
}