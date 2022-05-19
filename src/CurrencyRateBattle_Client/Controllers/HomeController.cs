using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json;
using CRBClient.Services.Interfaces;
using PagedList;
using PagedListExtensions = X.PagedList.PagedListExtensions;
using CRBClient.Helpers;

namespace CRBClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IRoomService _roomService;

        private readonly IUserService _userService;


        private readonly List<RoomViewModel> _list = new()
        {
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel(),
            new RoomViewModel()
        };

        public HomeController(ILogger<HomeController> logger,
            IRoomService roomService, IUserService userService)
        {
            _logger = logger;
            _roomService = roomService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Main(int? page)
        {
            //var room = await _roomService.GetRooms();
            var token = HttpContext.Session.GetString("token");
            var pageSize =  4;
            var pageIndex = (page ?? 1);
            var pageX = PagedListExtensions.ToPagedList(_list, pageIndex, pageSize);
            return View(pageX);
        }

        public async Task<IActionResult> Profile()
        {
            AccountInfoViewModel accountInfo;
            try
            {
                accountInfo = await _userService.GetAccountInfoAsync();
            }
            catch (CustomException)
            {
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

        public IActionResult Rooms()
        {
            IEnumerable<RoomViewModel>? rooms = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://gorest.co.in/public/v2/");
                //HTTP GET
                var responseTask = client.GetAsync("users");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();//ReadAsAsync<IList<RoomViewModel>>();
                    readTask.Wait();

                    rooms = JsonSerializer.Deserialize<IEnumerable<RoomViewModel>>(readTask.Result);
                }
                else
                {
                    //log response status here..

                    rooms = Enumerable.Empty<RoomViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(rooms);
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
}
