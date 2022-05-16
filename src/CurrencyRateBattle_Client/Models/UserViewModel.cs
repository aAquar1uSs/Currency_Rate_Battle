using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRBClient.Models;

public class UserViewModel
{
    [BindProperty]
    [DisplayName("Email")]
    [EmailAddress]
    public string Email { get; set; }

    [BindProperty]
    [DisplayName("Password")]
    [StringLength(30, MinimumLength = 6)]
    public string Password { get; set; }

    [BindProperty]
    [DisplayName("Confirm password")]
    public string ConfirmPassword { get; set; }
}
