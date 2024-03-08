using Certificate.Application.Abstractions.Interfaces;
using Certificate.Application.Abstractions.Interfaces.RepositoryServices;
using Certificate.Infrastructure.Persistence;
using Certificate.Infrastructure.Services.RepositoryServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Certificate.Infrastructure.Extensions;

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