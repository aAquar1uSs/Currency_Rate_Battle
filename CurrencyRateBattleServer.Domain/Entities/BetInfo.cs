namespace CurrencyRateBattleServer.Domain.Entities;

public class BetInfo
{
    public Guid Id { get; set; }

    public DateTime? SettleDate { get; set; }

    public string СurrencyName { get; set; } = default!;

    public DateTime RoomDate { get; set; }

    public decimal? WonCurrencyExchange { get; set; }

    public decimal UserCurrencyExchange { get; set; }

    public decimal BetAmount { get; set; }

    public decimal? PayoutAmount { get; set; }

    public DateTime SetDate { get; set; }

    public bool IsClosed { get; set; }
}
