using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal.Entities;

[Index(nameof(Email), IsUnique = true)]
public sealed class UserDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}
