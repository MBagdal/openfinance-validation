using OpenFinance.Validation.Application.Interfaces;

namespace OpenFinance.Validation.Infrastructure.Services;

/// <summary>
/// Implementação do serviço de GUID
/// </summary>
public class GuidService : IGuidService
{
    public string NewGuid() => Guid.NewGuid().ToString();
}