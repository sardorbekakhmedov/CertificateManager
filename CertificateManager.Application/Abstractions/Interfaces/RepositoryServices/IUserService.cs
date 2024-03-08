using Certificate.Application.DataTransferObjects.UserDTOs;
using Certificate.Application.Services.TokenServices;
using Certificate.Application.SortFilters.FilterEntities;
using CertificateManager.Domain.Entities;

namespace Certificate.Application.Abstractions.Interfaces.RepositoryServices;

public interface IUserService
{
    Task<Guid> CreateAsync(UserCreateDto dto);
    Task<TokenResponse> LoginAsync(TokenRequest request);
    Task<IEnumerable<UserDto>> GetAllAsync(UserFilter filter);
    Task<UserDto> GetByIdAsync(Guid userId);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<User> UpdateAsync(Guid userId, UserUpdateDto dto);
    Task DeleteAsync(Guid userId);
}