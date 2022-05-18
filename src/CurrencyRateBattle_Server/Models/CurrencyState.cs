using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Models;
public class CurrencyState
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    [Required]
    public DateTime Date { get; set; }

    public decimal? USDValue { get; set; }
    public decimal CurrencyExchangeRate { get; set; }

    [ForeignKey("RoomId")]
    public Guid RoomId { get; set; }

    public virtual Room Room { get; set; }

    [ForeignKey("CurrencyId")]
    public Guid CurrencyId { get; set; }

    public virtual Currency Currency { get; set; }
}
