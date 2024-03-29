﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Application.Exceptions;
using CertificateManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;

namespace CertificateManager.Application.Services.TokenServices;

public class JwtBearerService : ITokenService
{
    private readonly JwtBearerOption _options;
    private readonly IAppDbContext _dbContext;
    public JwtBearerService(
        IAppDbContext dbContext,
        IOptions<JwtBearerOption> options)
    {
        _dbContext = dbContext;
        _options = options.Value;
    }

    public async Task<TokenResponse> GetTokenAsync(TokenRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username)
            ?? throw new NotFoundException($"{nameof(User)} not found!.");

        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result != PasswordVerificationResult.Success)
            throw new BadRequestException("Password in valid!");

        var (token, tokenExpiryTime) = GenerateToken(user);

        return new TokenResponse()
        {
            UserId = user.Id, 
            Username = user.Username,
            ExpiresInMinutes = tokenExpiryTime,
            UserRole = user.UserRole.GetDisplayName(),
            Token = token
        };
    }

    public (string, double) GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role,user.UserRole.GetDisplayName()),
        };

        var signingKey = Encoding.UTF8.GetBytes(_options.SigningKey);
        var symmetricKey = new SymmetricSecurityKey(signingKey);
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: _options.ValidIssuer,
            audience: _options.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresTokenInMinutes),
            signingCredentials: signingCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        var expiresMinutes = TimeSpan.FromMinutes(_options.ExpiresTokenInMinutes).TotalMinutes;

        return new(token, expiresMinutes);
    }
}