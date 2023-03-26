using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Sockets;
using CRBClient.Services.Interfaces;
using CRBClient.Helpers;
using CRBClient.Dto;

namespace CRBClient.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRoomService _roomService;
    private readonly IUserService _userService;
    private readonly ICurrencyService _currencyService;
    private List<RoomViewModel> _roomStorage = new();

    public HomeController(ILogger<HomeController> logger,
        IRoomService roomService,
        IUserService userService,
        ICurrencyService currencyService)
    {
        _logger = logger;
        _roomService = roomService;
        _userService = userService;
        _currencyService = currencyService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Home page with all rooms");
        ViewBag.Balance = await _userService.GetUserBalanceAsync(cancellationToken);

        return View();
    }

    public async Task<IActionResult> Main(string searchNameString,
        string searchStartDateString,
        string searchEndDateString,
        int? page,
        CancellationToken cancellationToken)
    {
        try
        {
            ViewBag.Balance = await _userService.GetUserBalanceAsync(cancellationToken);
            var currState = await _currencyService.GetCurrencyRatesAsync(cancellationToken);
            ViewBag.CurrencyRates = currState;
            ViewBag.Title = "Main Page";

            ViewData["CurrentNameFilter"] = searchNameString;
            ViewData["CurrentStartDateFilter"] = searchStartDateString;
            ViewData["CurrentEndDateFilter"] = searchEndDateString;

            var filter = new FilterDto(searchNameString, searchStartDateString, searchEndDateString);
            _roomStorage = filter.CheckFilter()
                ? await _roomService.GetFilteredCurrencyAsync(filter, cancellationToken)
                : await _roomService.GetRoomsAsync(false, cancellationToken);
        }
        catch (GeneralException)
        {
            _logger.LogDebug("User unauthorized");
            return Redirect("/Account/Authorization");
        }
        catch (SocketException ex)
        {
            _logger.LogError("{Msg}", ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }

        var pageSize = 4;
        var roomViewModels = new PaginationList<RoomViewModel>();
        return View(await roomViewModels.CreateAsync(_roomStorage, page ?? 1, pageSize));
    }

    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
    {
        AccountInfoViewModel accountInfo;
        try
        {
            ViewBag.Balance = await _userService.GetUserBalanceAsync(cancellationToken);
            ViewBag.Title = "User Profile";

            accountInfo = await _userService.GetAccountInfoAsync(cancellationToken);
        }
        catch (GeneralException)
        {
            _logger.LogDebug("User unauthorized");
            return Redirect("/Account/Authorization");
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

        _logger.LogInformation("User profile page");
        return View(accountInfo);
    }

    public IActionResult Logout()
    {
        _userService.Logout();
        HttpContext.Session.Clear();

        _logger.LogInformation("User logout");
        return Redirect("/Account/Authorization");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

