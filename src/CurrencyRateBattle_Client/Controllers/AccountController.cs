using CRBClient.Helpers;
using CRBClient.Models;
using CRBClient.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;

    private readonly IUserService _userService;

    public AccountController(ILogger<AccountController> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public ActionResult Authorization()
    {
        ViewBag.Title = "Authorization - Currency Rate Battle";
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> LoginAsync(UserViewModel user)
    {
        try
        {
            await _userService.LoginUserAsync(user);
        }
        catch (CustomException ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
            return View("Authorization");
        }

        return Redirect("/Home/Main");
    }

    [HttpPost]
    public async Task<ActionResult> RegistrationAsync(UserViewModel user)
    {
        try
        {
            await _userService.RegisterUserAsync(user);
        }
        catch (CustomException ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
            return View("Authorization");
        }

        return Redirect("/Home/Main");
    }
}
