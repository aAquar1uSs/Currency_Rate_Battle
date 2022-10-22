using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CurrencyRateBattleServer.Dto;

public class RateDto
{
    [Required]
    [JsonPropertyName("roomId")]
    public Guid RoomId { get; set; }

    [Required]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [Required]
    [JsonPropertyName("userCurrencyExchange")]
    public decimal UserCurrencyExchange { get; set; }
}
