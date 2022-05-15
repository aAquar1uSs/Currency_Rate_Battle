using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattleServer.Dto;

public class UserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 6)]
    public string Password { get; set; }
}
