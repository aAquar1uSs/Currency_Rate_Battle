using System.ComponentModel.DataAnnotations;
namespace CurrencyRateBattleServer.Models;

public class Room
{
    [Key]
    public Guid Id { get; }

    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// IsClosed must be filled when DateTime is passed (with hosted service)
    /// </summary>

    public bool IsClosed { get; set; }
}
