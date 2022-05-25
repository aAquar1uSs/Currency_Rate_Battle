using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRBClient.Services.Interfaces;
using PagedList;
using CRBClient.Helpers;
using PagedListExtensions = X.PagedList.PagedListExtensions;
using CRBClient.Services;
using System.Globalization;

namespace CRBClient.Controllers
{
    public class RatingsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRatingService _ratingService;

        private readonly IUserService _userService;

        public RatingsController(ILogger<HomeController> logger,
            IRatingService ratingService,
            IUserService userService)
        {
            _logger = logger;
            _ratingService = ratingService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page, new CultureInfo("uk-UA")) : 1;
            ViewBag.Balance = await _userService.GetUserBalanceAsync();
            ViewBag.Title = "Users Ratings";
            try
            {
                var ratingInfo = await _ratingService.GetUserRatings();
                var ratings = PagedListExtensions.ToPagedList(ratingInfo, pageIndex, pageSize);
                return View(ratings);
            }
            catch (CustomException)
            {
                _logger.LogDebug("User is unauthorized");
                return Redirect("/Account/Authorization");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
