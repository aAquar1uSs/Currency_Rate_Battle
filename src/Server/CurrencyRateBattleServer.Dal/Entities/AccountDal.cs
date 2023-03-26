using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CurrencyRateBattleServer.Dal.Entities;

public sealed class AccountDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public decimal Amount { get; set; }
    
    public string Email { get; set; }

    [ForeignKey("Email")]
    public UserDal User { get; set; }
}
