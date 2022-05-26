using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRBClient.Services.Interfaces;
using CRBClient.Helpers;
using CRBClient.Dto;

namespace CRBClient.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IRoomService _roomService;

    private readonly IUserService _userService;

    private readonly ICurrencyStateService _currencyStateService;

    private List<RoomViewModel> _roomStorage = new();

    public HomeController(ILogger<HomeController> logger,
        IRoomService roomService,
        IUserService userService,
        ICurrencyStateService currencyStateService)
    {
        _logger = logger;
        _roomService = roomService;
        _userService = userService;
        _currencyStateService = currencyStateService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Balance = await _userService.GetUserBalanceAsync();

        return View();
    }

    public async Task<IActionResult> Main(string searchNameString,
        string searchStartDateString,
        string searchEndDateString,
        int? page)
    {
        try
        {
            ViewBag.Balance = await _userService.GetUserBalanceAsync();
            var currState = await _currencyStateService.GetCurrencyRatesAsync();
            ViewBag.CurrencyRates = currState;
            ViewBag.Title = "Main Page";

            ViewData["CurrentNameFilter"] = searchNameString;
            ViewData["CurrentStartDateFilter"] = searchStartDateString;
            ViewData["CurrentEndDateFilter"] = searchEndDateString;

            var filter = new FilterDto(searchNameString, searchStartDateString, searchEndDateString);
            _roomStorage = filter.CheckFilter() ?
                await _roomService.GetFilteredCurrencyAsync(filter)
                : await _roomService.GetRoomsAsync(false);
        }
        catch (CustomException)
        {
            _logger.LogDebug("User unauthorized");
            return Redirect("/Account/Authorization");
        }

        var pageSize = 4;
        return View(await PaginationList<RoomViewModel>.CreateAsync(_roomStorage, page ?? 1, pageSize));
    }

    public async Task<IActionResult> Profile()
    {
        AccountInfoViewModel accountInfo;
        try
        {
            ViewBag.Balance = await _userService.GetUserBalanceAsync();
            ViewBag.Title = "User Profile";

            accountInfo = await _userService.GetAccountInfoAsync();
        }
        catch (CustomException)
        {
            _logger.LogDebug("User unauthorized");
            return Redirect("/Account/Authorization");
        }

        return View(accountInfo);
    }

    public IActionResult Logout()
    {
        _userService.Logout();
        HttpContext.Session.Clear();
        return Redirect("/Account/Authorization");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
