using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Models;
public class Rate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    //Date when the rate is added by user
    [Required]
    public DateTime SetDate { get; set; }

    public decimal Amount { get; set; }

    //Date when the rate is settled
    public DateTime? SettleDate { get; set; }

    public decimal? Payout { get; set; }

    /// <summary>
    /// IsClosed must be filled when DateTime is passed (with hosted service)
    /// </summary>
    public bool IsClosed { get; set; }

    /// <summary>
    /// IsWon is true when the rate is winning; false - for losing rate
    /// </summary>

    public bool IsWon { get; set; }

    [ForeignKey("RoomId")]
    public Guid RoomId { get; set; }

    public virtual Room Room { get; set; }

    [ForeignKey("CurrencyId")]
    public Guid CurrencyId { get; set; }

    public virtual Currency Currency { get; set; }

    [ForeignKey("AccountId")]
    public Guid AccountId { get; set; }

    public virtual Account Account { get; set; }
}
