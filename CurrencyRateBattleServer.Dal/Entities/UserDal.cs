using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal.Entities;

[Index(nameof(Email), IsUnique = true)]
public sealed class UserDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    public string Email { get; set; }

    public string Password { get; set; }

    [ForeignKey("AccountId")]
    public Guid AccountId { get; set; }

    public AccountDal Account { get; set; }
}
