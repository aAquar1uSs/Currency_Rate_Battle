using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CurrencyRateBattleServer.Models;

public class Room
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// IsClosed must be filled when DateTime is passed (with hosted service)
    /// </summary>

    public bool IsClosed { get; set; }
}
