using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Models;

public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; }

    [Required]
    public decimal Amount { get; set; }

    [ForeignKey("UserId")]
    public Guid UserId { get; set; }

    public User User { get; set; } = default!;

}
