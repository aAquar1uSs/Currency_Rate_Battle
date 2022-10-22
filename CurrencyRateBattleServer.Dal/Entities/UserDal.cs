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
    
    public string Email { get; set; } = default!;
    
    public string Password { get; set; } = default!;

    public AccountDal Account { get; set; } = default!;
}
