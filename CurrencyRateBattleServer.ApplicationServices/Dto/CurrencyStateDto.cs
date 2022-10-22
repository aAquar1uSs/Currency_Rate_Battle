using System.Text.Json.Serialization;

namespace CurrencyRateBattleServer.Dto;

public class CurrencyStateDto
{
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    [JsonPropertyName("cc")]
    public string? Currency { get; set; }

    [JsonPropertyName("exchangedate")]
    public string? Date { get; set; }
}
