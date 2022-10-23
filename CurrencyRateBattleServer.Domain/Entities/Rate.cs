namespace CurrencyRateBattleServer.Domain.Entities;
public class Rate
{
    public Guid Id { get; set; }

    public DateTime SetDate { get; set; }

    public decimal RateCurrencyExchange { get; set; }

    public decimal Amount { get; set; }

    //Date when the rate is settled
    public DateTime? SettleDate { get; set; }

    public decimal? Payout { get; set; }

    public bool IsClosed { get; set; }

    public bool IsWon { get; set; }

    public  Room Room { get; set; } = default!;

    public  Currency Currency { get; set; } = default!;

    public  Account Account { get; set; } = default!;
}
