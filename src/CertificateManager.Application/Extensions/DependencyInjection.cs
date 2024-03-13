using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Application.Services;
using CertificateManager.Application.Services.DefaultSeedData;
using CertificateManager.Application.Services.Mappers;
using CertificateManager.Application.Services.PdfServices;
using CertificateManager.Application.Services.TokenServices;
using CertificateManager.Application.SortFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CertificateManager.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerConfiguration();

        services.AddAutoMapper(typeof(AutoMapperService));

        services.AddScoped<ITokenService, JwtBearerService>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IHttpContextHelper, HttpContextHelper>();
        services.AddScoped<IDefaultUserSeedData, DefaultUserSeedData>();
        services.AddScoped<IPdfCreatorService, PdfCreatorService>();

        services.AddHttpContextAccessor();

        return services;
    }

    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = @"JWT Bearer. : Authorization: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },

                    new List<string>(){}
                }
            });
        });
    }
}