using Certificate.Application.Abstractions.Interfaces;
using CertificateManager.Domain.Entities;
using CertificateManager.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Certificate.Application.Services.DefaultSeedData;

public class DefaultUserSeedData : IDefaultUserSeedData
{
    public async Task UserSeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
         
        var defaultUserId = Guid.Parse("11111111-2222-3333-4444-555555555555");
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