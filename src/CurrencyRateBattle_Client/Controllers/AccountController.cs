using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Controllers;
public class AccountController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
