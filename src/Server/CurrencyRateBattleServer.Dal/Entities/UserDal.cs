using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal.Entities;

public sealed class UserDal
{
    [Key]
    public string Email { get; set; }

    public string Password { get; set; }
}
