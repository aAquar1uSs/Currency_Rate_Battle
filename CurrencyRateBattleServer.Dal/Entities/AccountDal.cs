using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CurrencyRateBattleServer.Dal.Entities;

public sealed class AccountDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    public decimal Amount { get; set; }

    [ForeignKey("UserId")]
    public Guid UserId { get; set; }

    public UserDal User { get; set; }
}
