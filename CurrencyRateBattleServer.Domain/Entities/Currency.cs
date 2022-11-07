
namespace CurrencyRateBattleServer.Domain.Entities;

public class Currency
{
    public Guid Id { get; set; }

    public string CurrencyName { get; set; }

    public string CurrencySymbol { get; set; }
    
    public decimal Rate { get; set; }

    public string Description { get; set; }
}
