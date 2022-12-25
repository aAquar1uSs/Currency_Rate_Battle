using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Dal.Entities;
public class CurrencyStateDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    public DateTime UpdateDate { get; set; }

    public decimal CurrencyExchangeRate { get; set; }
    
    public Guid RoomId { get; set; }
    
    [ForeignKey("RoomId")]
    public RoomDal Room { get; set; }

    public string CurrencyName { get; set; }
    
    public string CurrencyCode { get; set; }
}
