using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRBClient.Services.Interfaces;
using CRBClient.Helpers;
using PagedListExtensions = X.PagedList.PagedListExtensions;
using System.Globalization;

namespace CRBClient.Controllers
{
    public class RatingsController : Controller
    {
        private readonly ILogger<RatingsController> _logger;

        private readonly IRatingService _ratingService;

        private readonly IUserService _userService;

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
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page, new CultureInfo("uk-UA")) : 1;
            ViewBag.Balance = await _userService.GetUserBalanceAsync();
            ViewBag.Title = "Users Ratings";
            try
            {
                var ratingInfo = await _ratingService.GetUserRatings();

                switch (sortOrder)
                {
                    case "bets_no":
                        ratingInfo = ratingInfo.OrderByDescending(s => s.BetsNo).ToList();
                        break;

                    case "bets_no_asc":
                        ratingInfo = ratingInfo.OrderBy(s => s.BetsNo).ToList();
                        break;

                    case "won_bets_no":
                        ratingInfo = ratingInfo.OrderByDescending(s => s.WonBetsNo).ToList();
                        break;

                    case "won_bets_no_asc":
                        ratingInfo = ratingInfo.OrderBy(s => s.WonBetsNo).ToList();
                        break;

                    case "profitperc":
                        ratingInfo = ratingInfo.OrderByDescending(s => s.ProfitPercentage).ToList();
                        break;

                    case "profitperc_asc":
                        ratingInfo = ratingInfo.OrderBy(s => s.ProfitPercentage).ToList();
                        break;

                    case "wonbetsperc":
                        ratingInfo = ratingInfo.OrderByDescending(s => s.WonBetsPercentage).ToList();
                        break;

                    case "wonbetsperc_asc":
                        ratingInfo = ratingInfo.OrderBy(s => s.WonBetsPercentage).ToList();
                        break;

                    default:
                        ratingInfo = ratingInfo.OrderByDescending(s => s.BetsNo).ToList();
                        break;
                }

                var ratings = PagedListExtensions.ToPagedList(ratingInfo, pageIndex, pageSize);
                return View(ratings);
            }
            catch (CustomException)
            {
                _logger.LogDebug("User is unauthorized");
                return Redirect("/Account/Authorization");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel {RequestId = ex.Message});
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
