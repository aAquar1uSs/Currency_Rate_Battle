using CRBClient.Models;
using CRBClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Controllers;
public class RoomsController : Controller
{
    private readonly ICRBServer _CBRservice;
    public RoomsController(ICRBServer CBRservice)
    {
        _CBRservice = CBRservice;
    }

    public IActionResult Index()
    {
        var rooms = _CBRservice.GetRooms().Result;

        return View(rooms);
    }
}
