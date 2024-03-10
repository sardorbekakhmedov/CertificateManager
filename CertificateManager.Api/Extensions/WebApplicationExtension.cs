using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Infrastructure.Persistence;

namespace CertificateManager.Api.Extensions;

public static class WebApplicationExtension
{
    public static void AutoMigrateAppDbContext(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        if (services.GetService<AppDbContext>() is null)
            throw new ArgumentNullException(nameof(AppDbContext), $"The Web Application Extension class failed to create an instance of the {nameof(AppDbContext)} class");

        var appDbContext = services.GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate();
    }

    public static void CreateDefaultUserSeedData(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var seeder = serviceProvider.GetRequiredService<IDefaultUserSeedData>();

        seeder.UserSeedDataAsync(serviceProvider).Wait();
    }
}