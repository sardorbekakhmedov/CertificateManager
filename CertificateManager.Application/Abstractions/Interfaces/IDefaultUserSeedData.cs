namespace Certificate.Application.Abstractions.Interfaces;

public interface IDefaultUserSeedData
{
    Task UserSeedDataAsync(IServiceProvider serviceProvider);
}