using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CRBClient.Services.Interfaces;
using PagedListExtensions = X.PagedList.PagedListExtensions;
using CRBClient.Helpers;

namespace CRBClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRoomService _roomService;

        private readonly IUserService _userService;

        private readonly ICommonService _commonService;

        private List<RoomViewModel> _roomStorage = new();

        public HomeController(ILogger<HomeController> logger,
            IRoomService roomService, IUserService userService, ICommonService commonService)
        {
            _logger = logger;
            _roomService = roomService;
            _userService = userService;
            _commonService = commonService;
        }

        public IActionResult Index()
        {
            ViewBag.Balance = _commonService.GetUserBalanceAsync();
            return View();
        }

        public async Task<IActionResult> Main(string currentFilter,
            string searchString,
            int? page)
        {
            X.PagedList.IPagedList<RoomViewModel> pageX;
            try
            {

                ViewBag.Balance = await _commonService.GetUserBalanceAsync();
                ViewBag.Title = "Main Page";

                if (searchString != null)
                    page = 1;
                else
                {
                    searchString = currentFilter;
                }

                ViewData["CurrentFilter"] = searchString;

                if (!string.IsNullOrEmpty(searchString))
                    _roomStorage = await _roomService.GetFilteredCurrencyAsync(searchString.ToUpperInvariant());
                else
                    _roomStorage = await _roomService.GetRoomsAsync(false);
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
                ViewBag.Balance = await _commonService.GetUserBalanceAsync();
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
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
