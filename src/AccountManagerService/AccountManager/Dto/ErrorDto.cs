using System.Text.Json.Serialization;

namespace AccountManager.Dto;

public class ErrorDto
{
    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
