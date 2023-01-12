namespace CurrencyRateBattleServer.Domain.Entities;

public class UserRating
{
    // Basic rating by number of bets
    public long BetsNo { get; set; }

    // Number of won bets
    public long WonBetsNo { get; set; }

    //Percentage of payout/bets amount
    public decimal ProfitPercentage { get; set; }

    //Percentage of won bets
    public decimal WonBetsPercentage { get; set; }

    public string Email { get; set; } = default!;

    public DateTime LastBetDate { get; set; }
}
