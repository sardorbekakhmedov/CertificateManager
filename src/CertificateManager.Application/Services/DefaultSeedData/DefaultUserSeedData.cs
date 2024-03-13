using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Domain.Entities;
using CertificateManager.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CertificateManager.Application.Services.DefaultSeedData;

public class DefaultUserSeedData : IDefaultUserSeedData
{
    public async Task UserSeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        var defaultUserId = StaticFields.AdminId;

        var password = "asd123";

        if (!dbContext.Users.Any())
        {
            var user = new User
            {
                Id = defaultUserId,
                Username = "admin",
                Age = 25,
                Email = "admin@gamil.com",
                HasCertificate = false,
                UserRole = EUserRoles.Admin,
                CreatedById = defaultUserId
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }
}