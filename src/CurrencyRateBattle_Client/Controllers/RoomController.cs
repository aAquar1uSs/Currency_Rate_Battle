﻿using CRBClient.Models;
using CRBClient.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRBClient.Controllers;
public class RoomsController : Controller
{
    private readonly IRoomService _roomService;
    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    public IActionResult Index()
    {
        var rooms = _roomService.GetRooms().Result;

        return View(rooms);
    }
}
