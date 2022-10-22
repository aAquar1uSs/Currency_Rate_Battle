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

    public RoomDal Room { get; set; }
    
    public CurrencyDal Currency { get; set; }
}
