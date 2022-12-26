using System.Text.Json.Serialization;

namespace CRBClient.Dto;

public class CurrencyDto
{
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    [JsonPropertyName("cc")]
    public string? Currency { get; set; }

    [JsonPropertyName("exchangedate")]
    public string? Date { get; set; }
}
