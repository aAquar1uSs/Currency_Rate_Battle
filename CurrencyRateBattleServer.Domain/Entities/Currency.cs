
namespace CurrencyRateBattleServer.Domain.Entities;

public class Currency
{
    public Guid Id { get; set; }

    public string CurrencyName { get; set; } = default!;

    public string CurrencySymbol { get; set; } = default!;

    public string Description { get; set; } = default!;
}
