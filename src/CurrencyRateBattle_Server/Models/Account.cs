using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle_Server.Models;

public sealed class Account
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }
}
