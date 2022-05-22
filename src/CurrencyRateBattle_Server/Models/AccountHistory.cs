using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Models;
public class AccountHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    [Required]
    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    //IsCredit = true for credit transactions; dalse - for debit transactions
    public bool IsCredit { get; set; }

    [ForeignKey("RoomId")]
    public Guid? RoomId { get; set; }

    public virtual Room? Room { get; set; }

    [ForeignKey("AccountId")]
    public Guid AccountId { get; set; }

    public virtual Account Account { get; set; }
}
