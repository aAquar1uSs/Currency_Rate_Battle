using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRBClient.Services.Interfaces;
using PagedList;
using CRBClient.Helpers;
using PagedListExtensions = X.PagedList.PagedListExtensions;


namespace CRBClient.Controllers
{
    public class AccountHistoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUserService _userService;

        private readonly ICommonService _commonService;


        public AccountHistoryController(ILogger<HomeController> logger,
            IUserService userService, ICommonService commonService)
        {
            _logger = logger;
            _userService = userService;
            _commonService = commonService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageSize = 10;
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.Balance = await _commonService.GetUserBalanceAsync();
            ViewBag.Title = "Account History";
            List<AccountHistoryViewModel> accountHistoryInfo;
            try
            {
                accountHistoryInfo = await _userService.GetAccountHistoryAsync();
                //accountHistories = accountHistoryInfo.ToPagedList(pageIndex, pageSize);
                var accountHistories = PagedListExtensions.ToPagedList(accountHistoryInfo, pageIndex, pageSize);
                return View(accountHistories);
            }
            catch (CustomException)
            {
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
