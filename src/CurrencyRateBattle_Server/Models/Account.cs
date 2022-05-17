using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Models;

public sealed class Account
{
    [Key]
    public Guid Id { get; }

    [Required]
    public decimal Amount { get; set; }

    [ForeignKey("UserId")]
    public Guid UserId { get; set; }

    public User User { get; set; }

}
