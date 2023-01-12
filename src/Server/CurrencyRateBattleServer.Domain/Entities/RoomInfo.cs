namespace CurrencyRateBattleServer.Domain.Entities;

public class RoomInfo
{
    public Guid Id { get; set; }

    public DateTime UpdateRateTime { get; set; }
    
    public int CountRates { get; set; }
    
    public string CurrencyName { get; set; } = default!;
    
    public decimal CurrencyExchangeRate { get; set; }

    public DateTime Date { get; set; }

    public bool IsClosed { get; set; }
}
