﻿using CertificateManager.Application.DataTransferObjects.UserDTOs;
using CertificateManager.Application.Services.TokenServices;
using CertificateManager.Application.SortFilters.FilterEntities;
using CertificateManager.Domain.Entities;

namespace CertificateManager.Application.Abstractions.Interfaces.RepositoryServices;

public interface IUserService
{
    Task<Guid> CreateAsync(UserCreateDto dto);
    Task<TokenResponse> LoginAsync(TokenRequest request);
    Task<IEnumerable<UserDto>> GetAllAsync(UserFilter filter);
    Task UpdateUserCertificateAsync(Guid certificateId, List<UserUpdateDto> dto);
    Task<UserDto> GetByIdAsync(Guid userId);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<User> UpdateAsync(Guid userId, UserUpdateDto dto);
    Task DeleteAsync(Guid userId);
}