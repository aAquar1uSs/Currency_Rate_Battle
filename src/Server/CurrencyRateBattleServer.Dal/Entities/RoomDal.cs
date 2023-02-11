using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Dal.Entities;

public class RoomDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsClosed { get; set; }

    public string CurrencyName { get; set; }

    [ForeignKey("CurrencyName")]
    public CurrencyDal Currency { get; set; }
}
