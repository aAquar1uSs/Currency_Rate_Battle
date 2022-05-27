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
        catch (CustomException ex)
        {
            _logger.LogDebug(ex.Message);
            ViewData["ErrorMessage"] = ex.Message;
            return View("Authorization");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel {RequestId = ex.Message});
        }
        catch(SocketException ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel {RequestId = ex.Message});
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
            _logger.LogDebug(ex.Message);
            ViewData["ErrorMessage"] = ex.Message;
            return View("Authorization");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel {RequestId = ex.Message});
        }
        catch(SocketException ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel {RequestId = ex.Message});
        }

        return Redirect("/Home/Main");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
