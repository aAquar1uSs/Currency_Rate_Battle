using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRBClient.Models;

public class User
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 6)]
    public string Password { get; set; }

    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
