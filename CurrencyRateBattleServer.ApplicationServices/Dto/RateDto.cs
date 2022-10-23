using System.Text.Json.Serialization;

namespace CurrencyRateBattleServer.Dto;

public class RateDto
{
    [JsonPropertyName("roomId")]
    public Guid RoomId { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("userCurrencyExchange")]
    public decimal UserCurrencyExchange { get; set; }
}
