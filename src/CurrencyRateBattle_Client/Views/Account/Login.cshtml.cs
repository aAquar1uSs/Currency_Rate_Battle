using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRBClient.Views.Account;

public class Login : PageModel
{
    [BindProperty]
    [DisplayName("Email")]
    public string Email { get; set; }

    [BindProperty]
    [DisplayName("Password")]
    public string Password { get; set; }

    [DisplayName("Confirm password")]
    public string ConfirmPassword { get; set; }

    public void OnGet()
    {

    }

    public IActionResult OnPost()
    {
        return null;
    }
}
