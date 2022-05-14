using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle_Server.Models;

public sealed class User
{
    [Key]
    public int Id { get; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public Account Bill { get; set; }
}
