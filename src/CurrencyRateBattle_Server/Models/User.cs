using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattleServer.Models;

public sealed class User
{
    [Key]
    public Guid Id { get; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public Account Account { get; set; }
}
