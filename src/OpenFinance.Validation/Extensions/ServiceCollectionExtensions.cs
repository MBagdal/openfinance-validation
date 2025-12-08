using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenFinance.Validation.Domain.Interfaces;
using OpenFinance.Validation.Domain.Services;
using OpenFinance.Validation.Application.Interfaces;
using OpenFinance.Validation.Application.Services;
using OpenFinance.Validation.Infrastructure.Auth;
using OpenFinance.Validation.Infrastructure.Http;
using OpenFinance.Validation.Infrastructure.Services;

namespace OpenFinance.Validation.Extensions;

/// <summary>
/// Extensões para facilitar o registro de serviços
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona os serviços de validação Open Finance
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <returns>Coleção de serviços para encadeamento</returns>
    public static IServiceCollection AddOpenFinanceValidation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Domain Services
        services.AddScoped<ClientCredentialsPermissionValidator>();
        
        // Application Services
        services.AddScoped<IResponseService, ResponseService>();
        services.AddScoped<IAudienceService, AudienceService>();
        services.AddScoped<IGuidService, GuidService>();
        
        // Infrastructure Services
        services.AddScoped<IAuthenticationValidator, AuthenticationValidator>();
        services.AddScoped<IHeaderValidator, HeaderValidator>();
        services.AddScoped<ICertificateService, CertificateService>();
        
        // Cliente principal
        services.AddScoped<OpenFinanceValidator>();
        
        return services;
    }
}