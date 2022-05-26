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
        ViewBag.Balance = await _userService.GetUserBalanceAsync();
        ViewBag.CurrencyName = currencyName;
        var rateModel = new RateViewModel
        {
            RoomId = roomId,
        };

        return View(rateModel);
    }

    public async Task<IActionResult> MakeBet(RateViewModel rateViewModel)
    {
        try
        {
            await _rateService.InsertUserRateAsync(rateViewModel);
        }
        catch (CustomException ex)
        {
            _logger.LogDebug(ex.Message);
            ViewData["ErrorMsg"] = ex.Message;
            return View("Index", rateViewModel);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogInformation(ex.Message);
            //ToDo Error page
        }

        return Redirect("/Home/Main");
    }
}
