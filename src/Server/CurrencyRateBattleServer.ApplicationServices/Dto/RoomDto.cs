using System.Text.Json.Serialization;

namespace CurrencyRateBattleServer.ApplicationServices.Dto;

public class RoomDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("updateRateTime")]
    public DateTime UpdateRateTime { get; set; }

    [JsonPropertyName("countRates")]
    public int CountRates { get; set; }

    [JsonPropertyName("currencyName")]
    public string СurrencyName { get; set; } = default!;

    [JsonPropertyName("currencyExchangeRate")]
    public decimal CurrencyExchangeRate { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("isClosed")]
    public bool IsClosed { get; set; }
}
