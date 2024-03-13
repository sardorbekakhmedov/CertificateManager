using System.Reflection;
using CertificateManager.Domain.Enums;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using CertificateManager.Application.Extensions;
using CertificateManager.Application.Services.TokenServices;
using CertificateManager.Infrastructure.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CertificateManager.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddCertificateManagerProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCertificateApiServices(configuration);
        services.AddApplicationServices(configuration);
        services.AddInfrastructureServices(configuration);

        return services;
    }

    public static IServiceCollection AddCertificateApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();

        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddJwtValidationService(configuration);

        services.AddMassTransit(c =>
        {
            c.AddConsumers(Assembly.GetEntryAssembly());

            c.UsingRabbitMq(
                (context, cfg) =>
                {
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

    public static void AddJwtValidationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtBearerOption>(configuration.GetSection("JwtBearerOption"));

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var configString = configuration["JwtBearerOption:SigningKey"];
                var validIssuer = configuration["JwtBearerOption:ValidIssuer"];
                var validAudience = configuration["JwtBearerOption:ValidAudience"];

                if (configString is null || validIssuer is null || validAudience is null)
                    throw new ArgumentNullException(nameof(configString));

                var signingKey = Encoding.UTF8.GetBytes(configString);

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization();
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