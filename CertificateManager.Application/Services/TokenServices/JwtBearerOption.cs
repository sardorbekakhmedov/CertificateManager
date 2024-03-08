namespace Certificate.Application.Services.TokenServices;

public class JwtBearerOption
{
    public required string SigningKey { get; set; }
    public required string ValidIssuer { get; set; }
    public required string ValidAudience { get; set; }
    public required int ExpiresTokenInMinutes { get; set; }
}