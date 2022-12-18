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
    
    public Guid RoomId { get; set; }
    
    [ForeignKey("RoomId")]
    public RoomDal Room { get; set; }
    
    public Guid CurrencyId { get; set; }

    [ForeignKey("CurrencyId")]
    public CurrencyDal Currency { get; set; }
}
