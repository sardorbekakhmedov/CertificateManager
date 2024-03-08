using Certificate.Application.Services.TokenServices;
using CertificateManager.Domain.Entities;

namespace Certificate.Application.Abstractions.Interfaces;

public interface ITokenService
{
    Task<TokenResponse> GetTokenAsync(TokenRequest request);
    (string, double) GenerateToken(User user);
}