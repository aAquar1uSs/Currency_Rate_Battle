using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle_Server.Models;

public sealed class Account
{
    [Key]
    public int Id { get;}

    [Required]
    public decimal Amount { get; set; }

    public int AccountRef { get; set; }

    public User User { get; set; }

}
