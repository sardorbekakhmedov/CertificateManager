﻿using System.Text.Json.Serialization;

namespace Certificate.Application.Services.TokenServices;

public class TokenResponse
{
    [JsonPropertyName("user_id")]
    public required Guid UserId { get; set; }

    [JsonPropertyName("user_name")]
    public required string Username { get; set; }

    [JsonPropertyName("token_expiry_time_in_minutes")]
    public required double ExpiresInMinutes { get; set; }
    
    [JsonPropertyName("token")]
    public required string Token { get; set; }
}
  