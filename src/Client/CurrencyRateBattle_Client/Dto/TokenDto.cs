using System.Text.Json.Serialization;

namespace CRBClient.Dto;

public class TokenDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = default!;

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = default!;
}
