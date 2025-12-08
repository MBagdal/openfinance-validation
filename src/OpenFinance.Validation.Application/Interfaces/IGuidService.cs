namespace OpenFinance.Validation.Application.Interfaces;

/// <summary>
/// Interface para geração de GUIDs
/// </summary>
public interface IGuidService
{
    /// <summary>
    /// Gera um novo GUID como string
    /// </summary>
    string NewGuid();
}