namespace CertificateManager.Application.Abstractions.Interfaces;

public interface IDefaultUserSeedData
{
    Task UserSeedDataAsync(IServiceProvider serviceProvider);
}