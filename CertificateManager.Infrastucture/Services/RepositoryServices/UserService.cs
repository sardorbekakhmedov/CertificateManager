﻿using AutoMapper;
using Certificate.Application.Abstractions.Interfaces;
using Certificate.Application.Abstractions.Interfaces.RepositoryServices;
using Certificate.Application.DataTransferObjects.UserDTOs;
using Certificate.Application.Exceptions;
using Certificate.Application.Extensions;
using Certificate.Application.Services.TokenServices;
using Certificate.Application.SortFilters.FilterEntities;
using CertificateManager.Domain.Entities;
using CertificateManager.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Certificate.Infrastructure.Services.RepositoryServices;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IAppDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly ICurrentUser _currentUser;
    private readonly IHttpContextHelper _httpContextHelper;

    public UserService(
        IMapper mapper, 
        IAppDbContext dbContext, 
        ITokenService tokenService,
        ICurrentUser currentUser,
        IHttpContextHelper httpContextHelper)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _httpContextHelper = httpContextHelper;
        _mapper = mapper;
        _currentUser = currentUser;
    }


    public async Task<Guid> CreateAsync(UserCreateDto dto)
    {
        if (await _dbContext.Users.AnyAsync(x => x.Username == dto.Username))
        {
            throw new BadRequestException($"User with username '{dto.Username}' already exists.");
        }

        var user = _mapper.Map<User>(dto);

        user.HasCertificate = false;
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, dto.Password);
        user.UserRole = EUserRoles.User;
        user.CreatedById = _currentUser.UserId;

        await _dbContext.BeginTransactionAsync();
        try
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            await _dbContext.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _dbContext.RollbackTransactionAsync();
            throw;
        }

        return user.Id;
    }



    public async Task<TokenResponse> LoginAsync(TokenRequest request)
    {
        return await _tokenService.GetTokenAsync(request);
    }

    public async Task<UserDto> GetByIdAsync(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId)
                   ?? throw new NotFoundException($"User not found! userId:  {userId}");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == username)
               ?? throw new NotFoundException($"User not found! Username:  {username}");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(UserFilter filter)
    {
        var query = _dbContext.Users.AsNoTracking().AsQueryable();

        if (filter.Username is not null)
            query = query.Where(u => u.Username.ToLower() == filter.Username.ToLower());

        if (filter.Age is not null)
            query = query.Where(u => u.Age == filter.Age);

        if (filter.HasCertificate is not null)
            query = query.Where(u => u.HasCertificate == filter.HasCertificate);

        if (filter.FromDateTime is not null)
            query = query.Where(u => u.CreatedDate >= filter.FromDateTime);

        if (filter.ToDateTime is not null)
            query = query.Where(u => u.CreatedDate <= filter.ToDateTime);

        var users = await query.ToPagedListAsync(_httpContextHelper, filter);

        return users.Select(user => _mapper.Map<UserDto>(user));
    }

    public async Task<User> UpdateAsync(Guid userId, UserUpdateDto dto)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be null!.");
        }

        await _dbContext.BeginTransactionAsync();
        try
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId)
                       ?? throw new NotFoundException($"User not found! userId: {userId}");

            user.Username = dto.Username ?? user.Username;
            user.Age = dto.Age ?? user.Age;
            user.Email = dto.Email ?? user.Email;
            user.UserRole = dto.UserRole ?? user.UserRole;

            _dbContext.Users.Attach(user);
            await _dbContext.SaveChangesAsync();

            await _dbContext.CommitTransactionAsync();

            return user;
        }
        catch (Exception)
        {
            await _dbContext.RollbackTransactionAsync();
            throw;
        }
    }


    public async Task DeleteAsync(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId)
                   ?? throw new NotFoundException($"User not found! userId:  {userId}");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

}