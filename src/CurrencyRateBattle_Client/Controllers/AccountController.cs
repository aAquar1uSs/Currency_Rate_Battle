using System.Diagnostics;
using System.Net.Sockets;
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
        catch (GeneralException ex)
        {
            _logger.LogInformation("User {User}: {Msg}", user.Email, ex.Message);
            ViewData["ErrorMessage"] = ex.Message;
            return View("Authorization");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("User {User}: {Msg}", user.Email, ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
        catch (SocketException ex)
        {
            _logger.LogError("User {User}: {Msg}", user.Email, ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }

        _logger.LogInformation($"User '{user.Email}' has successfully logged in");
        return Redirect("/Home/Main");
    }

    [HttpPost]
    public async Task<ActionResult> RegistrationAsync(UserViewModel user)
    {
        if (!ModelState.IsValid)
        {
            return View("Authorization");
        }

        try
        {
            await _userService.RegisterUserAsync(user);
        }
        catch (GeneralException ex)
        {
            _logger.LogInformation("User {User}: {Msg}", user.Email, ex.Message);
            ViewData["ErrorMessage"] = ex.Message;
            return View("Authorization");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("User {User}: {Msg}", user.Email, ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
        catch (SocketException ex)
        {
            _logger.LogError("User {User}: {Msg}", user.Email, ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }

        return Redirect("/Home/Main");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
