using System.Reflection;
using CertificateManager.Domain.Enums;
using System.Security.Claims;
using System.Text.Json.Serialization;
using CertificateManager.Application.DataTransferObjects.UserDTOs;
using CertificateManager.Application.Extensions;
using CertificateManager.Infrastructure.Extensions;
using MassTransit;

namespace CertificateManager.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddCertificateManagerProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCertificateApiServices();
        services.AddApplicationServices(configuration);
        services.AddInfrastructureServices(configuration);

        return services;
    }

    public static IServiceCollection AddCertificateApiServices(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddMassTransit(c =>
        {
            //var entryAssembly = Assembly.GetEntryAssembly();
            //c.AddConsumers(entryAssembly);

            c.UsingRabbitMq(
                (context, cfg) =>
                {
                    //cfg.Publish<UserUpdateListMessage> (d =>
                    //{
                    //    d.Durable = true;
                    //    d.ExchangeType = "topic";
                    //});

                    cfg.ConfigureEndpoints(context);
                }
            );
        });

        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

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