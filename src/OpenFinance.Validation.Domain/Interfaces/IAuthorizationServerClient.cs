using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Domain.Interfaces;

/// <summary>
/// Interface para cliente do servidor de autorização
/// </summary>
public interface IAuthorizationServerClient
{
    /// <summary>
    /// Faz introspecção do token de acesso
    /// </summary>
    Task<TokenDetails?> IntrospectAccessTokenAsync(string authorizationHeader);
    
    /// <summary>
    /// Obtém detalhes do cliente
    /// </summary>
    Task<ClientDetails> GetClientDetailsAsync(string clientId);
}

/// <summary>
/// Detalhes do cliente OAuth2
/// </summary>
public class ClientDetails
{
    public string JwksUri { get; set; } = string.Empty;
}