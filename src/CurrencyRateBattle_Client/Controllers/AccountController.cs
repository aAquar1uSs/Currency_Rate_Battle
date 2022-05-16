using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    
    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View("LoginView");
    }

    [HttpPost]
    public ActionResult Login(UserViewModel user)
    {
        return Redirect("/Home/Index");
    }

    [HttpPost]
    public ActionResult Registration(UserViewModel user)
    {
        return Redirect("/Home/Index");
    }
}
