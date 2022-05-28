using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRBClient.Services.Interfaces;
using CRBClient.Helpers;
using PagedListExtensions = X.PagedList.PagedListExtensions;
using System.Globalization;
using System.Net.Sockets;

namespace CRBClient.Controllers;

public class RatingsController : Controller
{
    private readonly ILogger<RatingsController> _logger;

    private readonly IRatingService _ratingService;

    private readonly IUserService _userService;

    private const int PageSize = 10;

    public RatingsController(ILogger<RatingsController> logger,
        IRatingService ratingService,
        IUserService userService)
    {
        _logger = logger;
        _ratingService = ratingService;
        _userService = userService;
    }

    public async Task<IActionResult> Index(int? page, string sortOrder)
    {
        ViewBag.CurrentSortOrder = sortOrder;
        ViewBag.BetNoSortParm = sortOrder == "bets_no" ? "bets_no_asc" : "bets_no";
        ViewBag.WonBetNoSortParm = sortOrder == "won_bets_no" ? "won_bets_no_asc" : "won_bets_no";
        ViewBag.ProfitPercSortParm = sortOrder == "profitperc" ? "profitperc_asc" : "profitperc";
        ViewBag.WonBetsPercSortParm = sortOrder == "wonbetsperc" ? "wonbetsperc_acs" : "wonbetsperc";
        ViewBag.Balance = await _userService.GetUserBalanceAsync();
        ViewBag.Title = "Users Ratings";

        try
        {
            var ratingInfo = await _ratingService.GetUserRatings();

            _ratingService.RatingListSorting(ref ratingInfo, sortOrder);

            var pageIndex = page.HasValue ? Convert.ToInt32(page, new CultureInfo("uk-UA")) : 1;
            var ratings = PagedListExtensions.ToPagedList(ratingInfo, pageIndex, PageSize);
            return View(ratings);
        }
        catch (GeneralException)
        {
            _logger.LogDebug("User is unauthorized");
            return Redirect("/Account/Authorization");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
        catch(SocketException ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
