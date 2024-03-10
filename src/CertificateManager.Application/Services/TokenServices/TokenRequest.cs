namespace CertificateManager.Application.Services.TokenServices;

public class TokenRequest
{
    public required string  Username { get; set; }
    public required string  Password { get; set; }
}