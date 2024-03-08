using System.Text;
using Certificate.Application.Abstractions.Interfaces;
using Certificate.Application.Services;
using Certificate.Application.Services.DefaultSeedData;
using Certificate.Application.Services.Mappers;
using Certificate.Application.Services.TokenServices;
using Certificate.Application.SortFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Certificate.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerConfiguration();
        services.AddJwtValidationService(configuration);

        services.AddAutoMapper(typeof(AutoMapperService));

        services.AddScoped<ITokenService, JwtBearerService>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IHttpContextHelper, HttpContextHelper>();
        services.AddScoped<IDefaultUserSeedData, DefaultUserSeedData>();

        services.AddHttpContextAccessor();

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