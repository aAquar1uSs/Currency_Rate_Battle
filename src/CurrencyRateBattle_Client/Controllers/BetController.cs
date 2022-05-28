using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRBClient.Services.Interfaces;
using CRBClient.Helpers;
using PagedListExtensions = X.PagedList.PagedListExtensions;
using System.Globalization;
using System.Net.Sockets;

namespace CRBClient.Controllers;

public class BetController : Controller
{
    private readonly ILogger<BetController> _logger;

    private readonly IUserService _userService;

    private readonly IUserRateService _userRateService;

    public BetController(ILogger<BetController> logger,
        IUserService userService, IUserRateService userRateService)
    {
        _logger = logger;
        _userService = userService;
        _userRateService = userRateService;
    }

    public async Task<IActionResult> Index(int? page)
    {
        var pageSize = 10;
        var pageIndex = page.HasValue ? Convert.ToInt32(page, new CultureInfo("uk-UA")) : 1;

        ViewBag.Balance = await _userService.GetUserBalanceAsync();
        ViewBag.Title = "My Bets";

        try
        {
            var betInfo = await _userRateService.GetUserRates();
            var bets = PagedListExtensions.ToPagedList(betInfo, pageIndex, pageSize);
            _logger.LogInformation("User betting page");
            return View(bets);
        }
        catch (GeneralException)
        {
            _logger.LogDebug("User is unauthorized");
            return Redirect("/Account/Authorization");
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
    }
        public async Task<IActionResult> Index(int? page)
        {
            var pageSize = 5;
            var pageIndex = page.HasValue ? Convert.ToInt32(page, new CultureInfo("uk-UA")) : 1;
            ViewBag.Balance = await _userService.GetUserBalanceAsync();
            ViewBag.Title = "My Bets";
            try
            {
                var betInfo = await _userRateService.GetUserRates();
                var bets = PagedListExtensions.ToPagedList(betInfo, pageIndex, pageSize);
                return View(bets);
            }
            catch (GeneralException)
            {
                _logger.LogDebug("User is unauthorized");
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
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
