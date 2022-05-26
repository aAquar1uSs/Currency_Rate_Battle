using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Models;

public class UserViewModel
{
    [BindProperty]
    [DisplayName("Email")]
    public string Email { get; set; } = default!;

    [BindProperty]
    [DisplayName("Password")]
    public string Password { get; set; } = default!;

    [BindProperty]
    [DisplayName("Confirm password")]
    public string? ConfirmPassword { get; set; }
}
