using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CRBClient.Models;

public class RateViewModel
{
    [JsonPropertyName("roomId")]
    public Guid RoomId { get; set; }

    [DisplayName("Amount")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [DisplayName("Your currency exchange")]
    [JsonPropertyName("userCurrencyExchange")]
    public decimal UserCurrencyExchange { get; set; }
}
