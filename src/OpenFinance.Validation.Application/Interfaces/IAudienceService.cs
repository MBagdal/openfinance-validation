namespace OpenFinance.Validation.Application.Interfaces;

/// <summary>
/// Interface para serviços de audience
/// </summary>
public interface IAudienceService
{
    /// <summary>
    /// Obtém o audience baseado no path e parâmetros
    /// </summary>
    string GetAudience(string path, string? audienceParams);
}