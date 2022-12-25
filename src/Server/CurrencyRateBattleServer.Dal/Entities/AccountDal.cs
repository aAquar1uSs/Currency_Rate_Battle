using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CurrencyRateBattleServer.Dal.Entities;

public sealed class AccountDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public decimal Amount { get; set; }
    
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public UserDal User { get; set; }
}
