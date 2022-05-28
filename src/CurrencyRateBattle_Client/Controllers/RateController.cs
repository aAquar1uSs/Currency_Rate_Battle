using System.Diagnostics;
using System.Net.Sockets;
using CRBClient.Helpers;
using CRBClient.Models;
using CRBClient.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Controllers;

public class RateController : Controller
{
    private readonly ILogger<RateController> _logger;

    private readonly IUserRateService _rateService;

    private readonly IUserService _userService;

    public RateController(ILogger<RateController> logger,
        IUserRateService rateService,
        IUserService userService)
    {
        _logger = logger;
        _rateService = rateService;
        _userService = userService;
    }

    public async Task<IActionResult> Index(Guid roomId, string currencyName)
    {
        ViewBag.Title = "Make Bet";
        ViewBag.Balance = await _userService.GetUserBalanceAsync();
        ViewBag.CurrencyName = currencyName;

        var rateModel = new RateViewModel
        {
            RoomId = roomId
        };

        return View(rateModel);
    }

    public async Task<IActionResult> MakeBet(RateViewModel rateViewModel, string currencyName)
    {
        ViewBag.Title = "Make Bet";
        ViewBag.Balance = await _userService.GetUserBalanceAsync();

        try
        {
            ViewBag.CurrencyName = currencyName;
            ViewBag.BalanceDecimal = await _userService.GetUserBalanceDecimalAsync();

            if (rateViewModel.Amount > ViewBag.BalanceDecimal)
            {
                ViewData["ErrorMsg"] = "You don't have enough funds on your account for making this bet.";
                _logger.LogInformation("User does not have enough money on the account" +
                    $" to make bet of {rateViewModel.Amount}UAH");
                return View("Index", rateViewModel);
            }
            if (rateViewModel.Amount <= 0 || rateViewModel.UserCurrencyExchange <= 0)
            {
                ViewData["ErrorMsg"] = "Invalid data";
                _logger.LogInformation("Invalid rate data");
                return View("Index", rateViewModel);
            }

            await _rateService.InsertUserRateAsync(rateViewModel);
        }
        catch (GeneralException ex)
        {
            _logger.LogDebug("{Msg}", ex.Message);
            ViewData["ErrorMsg"] = ex.Message;
            ViewBag.Balance = await _userService.GetUserBalanceAsync();
            return View("Index", rateViewModel);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("{Msg}", ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
        catch (SocketException ex)
        {
            _logger.LogError("{Msg}", ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }

        _logger.LogInformation($"User successfully placed a bet on {currencyName}: {rateViewModel.Amount}UAH");
        return Redirect("/Home/Main");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
