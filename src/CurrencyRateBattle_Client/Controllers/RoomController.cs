using CRBClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace CRBClient.Controllers;
public class RoomsController : Controller
{
    public IActionResult Index()
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
}
