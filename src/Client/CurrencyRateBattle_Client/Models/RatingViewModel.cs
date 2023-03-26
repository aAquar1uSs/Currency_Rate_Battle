using Newtonsoft.Json;

namespace CRBClient.Models;

public class RatingViewModel
{
    // 1 - Basic rating, by number of bets;
    // 2 - percentage of payout/bets amount;
    // 3 - Number of won bets;
    // 4 - Percentage of won bets

    // Basic rating by number of bets
    [JsonProperty("betsNo")]
    public long BetsNo { get; set; }

    // Number of won bets
    [JsonProperty("wonBetsNo")]
    public long WonBetsNo { get; set; }

    //Percentage of payout/bets amount
    [JsonProperty("profitPercentage")]
    public decimal ProfitPercentage { get; set; }

    //Percentage of won bets
    [JsonProperty("wonBetsPercentage")]
    public decimal WonBetsPercentage { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; } = default!;

    [JsonProperty("lastBetDate")]
    public DateTime LastBetDate { get; set; }
}
