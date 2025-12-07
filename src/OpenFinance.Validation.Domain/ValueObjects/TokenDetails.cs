namespace OpenFinance.Validation.Domain.ValueObjects;

/// <summary>
/// Detalhes do token de acesso OAuth2
/// </summary>
public class TokenDetails
{
    public bool Active { get; init; }
    public string Scope { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public Dictionary<string, string> Cnf { get; init; } = new();
    
    /// <summary>
    /// Verifica se o token possui um escopo específico
    /// </summary>
    public bool HasScope(string scope) => Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(scope);
}