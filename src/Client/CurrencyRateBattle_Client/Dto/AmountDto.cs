using System.Text.Json.Serialization;

namespace CRBClient.Dto;

public class AmountDto
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}
