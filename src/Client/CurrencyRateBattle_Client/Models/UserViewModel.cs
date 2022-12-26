using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Models;

public class UserViewModel
{
    [BindProperty]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required.")]
    [DisplayName("Email")]
    public string Email { get; set; } = default!;

    [BindProperty]
    [DisplayName("Password")]
    [StringLength(30, MinimumLength = 6)]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = default!;

    [BindProperty]
    [DisplayName("Confirm password")]
    [StringLength(30, MinimumLength = 6)]
    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Password doesn't match.")]
    public string? ConfirmPassword { get; set; }
}
