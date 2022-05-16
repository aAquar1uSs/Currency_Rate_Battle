using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Controllers;

public class AccountController : Controller
{
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
}
