using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CurrencyRateBattleServer.Models;

public class Room
{
    [Key]
    public int Id { get; }

    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// CurrencyStateRef and IsClosed must be filled when DateTime is passed (with hosted service)
    /// </summary>
    public int? CurrencyStateId { get; set; }

    [ForeignKey("CurrencyStateId")]
    public virtual CurrencyState? CurrencyState { get; set; }

    public bool IsClosed { get; set; }
}
