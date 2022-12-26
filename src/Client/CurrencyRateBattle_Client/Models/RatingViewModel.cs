using Newtonsoft.Json;

namespace CRBClient.Models;

public class RatingViewModel
{
    // 1 - Basic rating, by number of bets;
    // 2 - percentage of payout/bets amount;
    // 3 - Number of won bets;
    // 4 - Percentage of won bets

    // Basic rating by number of bets
    [JsonProperty("BetsNo")]
    public long BetsNo { get; set; }

    // Number of won bets
    [JsonProperty("WonBetsNo")]
    public long WonBetsNo { get; set; }

    //Percentage of payout/bets amount
    [JsonProperty("ProfitPercentage")]
    public decimal ProfitPercentage { get; set; }

    //Percentage of won bets
    [JsonProperty("WonBetsPercentage")]
    public decimal WonBetsPercentage { get; set; }

    [JsonProperty("Email")]
    public string Email { get; set; } = default!;

    [JsonProperty("LastBetDate")]
    public DateTime LastBetDate { get; set; }
}
