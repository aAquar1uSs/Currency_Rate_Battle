using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Dal.Entities;
public class AccountHistoryDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public bool IsCredit { get; set; }

    [ForeignKey("RoomId")]
    public Guid? RoomId { get; set; }

    public RoomDal? Room { get; set; }

    [ForeignKey("AccountId")]
    public AccountDal Account { get; set; }
}
