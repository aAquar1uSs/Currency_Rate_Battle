namespace CurrencyRateBattleServer.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public bool IsClosed { get; set; }
}
