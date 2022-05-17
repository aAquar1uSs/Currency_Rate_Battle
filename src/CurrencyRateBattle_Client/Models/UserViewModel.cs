using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRBClient.Models;

public class UserViewModel
{
    [BindProperty]
    [DisplayName("Email")]
    public string Email { get; set; }

    [BindProperty]
    [DisplayName("Password")]
    public string Password { get; set; }

    [BindProperty]
    [DisplayName("Confirm password")]
    public string? ConfirmPassword { get; set; }
}
