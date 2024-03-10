using CertificateManager.Application.Services.TokenServices;
using CertificateManager.Domain.Entities;

namespace CertificateManager.Application.Abstractions.Interfaces;

public interface ITokenService
{
    Task<TokenResponse> GetTokenAsync(TokenRequest request);
    (string, double) GenerateToken(User user);
}