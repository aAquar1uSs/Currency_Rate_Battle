namespace CurrencyRateBattleServer.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    
    public DateTime Date { get; set; }

    /// <summary>
    /// IsClosed must be filled when DateTime is passed (with hosted service)
    /// </summary>

    public bool IsClosed { get; set; }
}
