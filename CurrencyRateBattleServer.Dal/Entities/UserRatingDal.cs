namespace CurrencyRateBattleServer.Data;

public class UserRatingDal
{
    public string UserEmail { get; set; }

    public Guid AccountId { get; set; }

    public decimal? BetAmount { get; set; }

    public bool IsWon { get; set; }

    public DateTime RateSetDate { get; set; }

    public decimal? RatePayout { get; set; }

    public DateTime LastBetDate { get; set; }

    public decimal? TotalBetAmount { get; set; }

    public decimal? TotalPayout { get; set; }
    public decimal? WonBetAmount { get; set; }
    public decimal? WonPayout { get; set; }

    public int TotalBetCount { get; set; }

    public int WonBetCount { get; set; }

}
