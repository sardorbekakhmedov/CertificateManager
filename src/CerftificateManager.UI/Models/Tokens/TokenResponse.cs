using System.Text.Json.Serialization;
using CertificateManager.UI.Models.Enums;

namespace CertificateManager.UI.Models.Tokens;

public class TokenResponse
{
    [JsonPropertyName("user_id")]
    public required Guid UserId { get; set; }

    [JsonPropertyName("user_name")]
    public required string Username { get; set; }

    [JsonPropertyName("token_expiry_time_in_minutes")]
    public required double ExpiresInMinutes { get; set; }

    [JsonPropertyName("user-role")]
    public required string UserRole { get; set; }

    [JsonPropertyName("token")]
    public required string Token { get; set; }
}
