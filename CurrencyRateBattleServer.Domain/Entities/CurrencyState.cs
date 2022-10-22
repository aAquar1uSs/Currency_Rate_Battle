namespace CurrencyRateBattleServer.Domain.Entities;
public class CurrencyState
{
    public Guid Id { get; }
    
    public DateTime Date { get; set; }

    public decimal CurrencyExchangeRate { get; set; }

    public Room Room { get; set; } = default!;

    public Currency Currency { get; set; } = default!;
}
