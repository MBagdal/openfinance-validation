using Microsoft.Extensions.Configuration;
using OpenFinance.Validation.Application.Interfaces;

namespace OpenFinance.Validation.Application.Services;

/// <summary>
/// Serviço para construção de audience
/// </summary>
public class AudienceService : IAudienceService
{
    private readonly IConfiguration _configuration;

    public AudienceService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetAudience(string path, string? audienceParams)
    {
        var prefix = _configuration["OpenFinance:Validation:AudiencePrefix"] ?? string.Empty;
        return $"{prefix}{path}";
    }
}