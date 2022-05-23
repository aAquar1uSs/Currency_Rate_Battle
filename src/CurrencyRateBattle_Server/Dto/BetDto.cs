namespace CurrencyRateBattleServer.Dto;

public class BetDto
{
    public Guid Id { get; set; }

    public DateTime? SettleDate { get; set; }

    public string СurrencyName { get; set; }

    public DateTime RoomDate { get; set; }

    public decimal? WonCurrencyExchange { get; set; }

    public decimal UserCurrencyExchange { get; set; }

    public decimal BetAmount { get; set; }

    public DateTime SetDate { get; set; }

    public bool IsClosed { get; set; }

}
