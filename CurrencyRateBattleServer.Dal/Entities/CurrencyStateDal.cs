using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Dal.Entities;
public class CurrencyStateDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    public DateTime Date { get; set; }

    public decimal CurrencyExchangeRate { get; set; }

    [ForeignKey("RoomId")]
    public Guid RoomId { get; set; }
    public RoomDal Room { get; set; }

    [ForeignKey("CurrencyId")]
    public Guid CurrencyId { get; set; }

    public CurrencyDal Currency { get; set; }
}
