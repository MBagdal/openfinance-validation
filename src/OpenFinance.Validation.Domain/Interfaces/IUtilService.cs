namespace OpenFinance.Validation.Domain.Interfaces;

/// <summary>
/// Interface para serviços utilitários
/// </summary>
public interface IUtilsService
{
    /// <summary>
    /// Extrai o ID da organização do URI do JWKS
    /// </summary>
    string ExtractOrgIdFromJwksUri(string jwksUri);
}