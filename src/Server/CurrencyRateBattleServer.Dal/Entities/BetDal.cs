namespace CurrencyRateBattleServer.Dal.Entities;

public class BetDal
{
    public Guid RateId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? RateSettleDate { get; set; }

    public DateTime RateSetDate { get; set; }

    public bool IsWon { get; set; }

    public bool IsClosed { get; set; }

    public Guid AccountId { get; set; }

    public decimal RateCurrencyExchange { get; set; }

    public decimal? Payout { get; set; }

    public DateTime RoomDate { get; set; }

    public Guid RoomId { get; set; }

    public string CurrencyName { get; set; }

    public decimal? CurrencyExchangeRate { get; set; }
}
