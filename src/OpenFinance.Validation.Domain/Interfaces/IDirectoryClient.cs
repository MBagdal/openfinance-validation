using Microsoft.IdentityModel.Tokens;

namespace OpenFinance.Validation.Domain.Interfaces;

/// <summary>
/// Interface para cliente do diretório
/// </summary>
public interface IDirectoryClient
{
    /// <summary>
    /// Obtém as chaves públicas do cliente (JWKS)
    /// </summary>
    Task<JsonWebKeySet> GetClientKeysAsync(ClientDetails client);
}