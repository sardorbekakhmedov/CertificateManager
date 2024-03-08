using CertificateManager.Domain.Enums;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Certificate.Application.Extensions;
using Certificate.Infrastructure.Extensions;
using CertificateManager.Api.PdfServices;

namespace CertificateManager.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddCertificateApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddApplicationServices(configuration);
        services.AddInfrastructureServices(configuration);

        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddScoped<DocumentCreator>();

        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddRoleServices();

        return services;
    }

    public static void AddRoleServices(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(EUserRoles.Admin.ToString(), configurePolicy =>
            {
                configurePolicy.RequireAssertion(
                    handler => handler.User.HasClaim(ClaimTypes.Role, EUserRoles.Admin.ToString()));
            });

            options.AddPolicy(EUserRoles.SuperUser.ToString(), configurePolicy =>
                configurePolicy.RequireAssertion(
                    handler => handler.User.HasClaim(ClaimTypes.Role, EUserRoles.SuperUser.ToString()) 
                               || handler.User.HasClaim(ClaimTypes.Role, EUserRoles.Admin.ToString())));
            
            options.AddPolicy(EUserRoles.User.ToString(), configurePolicy =>
            {
                configurePolicy.RequireAssertion(
                    handler => handler.User.HasClaim(ClaimTypes.Role, EUserRoles.SuperUser.ToString())
                               || handler.User.HasClaim(ClaimTypes.Role, EUserRoles.Admin.ToString())
                               || handler.User.HasClaim(ClaimTypes.Role, EUserRoles.User.ToString()));

            });
        });
    }
}