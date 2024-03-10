using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Application.Abstractions.Interfaces.RepositoryServices;
using CertificateManager.Infrastructure.Persistence;
using CertificateManager.Infrastructure.Services.RepositoryServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CertificateManager.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch(switchName: "Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);
        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
        {
            options.UseSnakeCaseNamingConvention()
             .UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICertificateService, CertificateService>();


        return services;
    }
}