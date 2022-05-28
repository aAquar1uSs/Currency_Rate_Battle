namespace CRBClient.Models;

public class RatingViewModel
{
    // 1 - Basic rating, by number of bets;
    // 2 - percentage of payout/bets amount;
    // 3 - Number of won bets;
    // 4 - Percentage of won bets

    // Basic rating by number of bets
    public long BetsNo { get; set; }

    // Number of won bets
    public long WonBetsNo { get; set; }

    //Percentage of payout/bets amount
    public decimal ProfitPercentage { get; set; }

    //Percentage of won bets
    public decimal WonBetsPercentage { get; set; }

    public string? Email { get; set; }

    public DateTime LastBetDate { get; set; }
}
