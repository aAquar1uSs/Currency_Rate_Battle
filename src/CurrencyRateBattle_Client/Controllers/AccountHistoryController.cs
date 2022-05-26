using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRBClient.Services.Interfaces;
using CRBClient.Helpers;
using PagedListExtensions = X.PagedList.PagedListExtensions;
using System.Globalization;

namespace CRBClient.Controllers
{
    public class AccountHistoryController : Controller
    {
        private readonly ILogger<AccountHistoryController> _logger;

        private readonly IUserService _userService;

        public AccountHistoryController(ILogger<AccountHistoryController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page, new CultureInfo("uk-UA")) : 1;
            ViewBag.Balance = await _userService.GetUserBalanceAsync();
            ViewBag.Title = "Account History";
            try
            {
                var accountHistoryInfo = await _userService.GetAccountHistoryAsync();
                //accountHistories = accountHistoryInfo.ToPagedList(pageIndex, pageSize);
                var accountHistories = PagedListExtensions.ToPagedList(accountHistoryInfo, pageIndex, pageSize);
                return View(accountHistories);
            }
            catch (CustomException)
            {
                _logger.LogDebug("User unauthorized");
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
