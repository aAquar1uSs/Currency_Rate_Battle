using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CurrencyRateBattleServer.Controllers
{
    [Route("api/room")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;

        private readonly IRoomService _roomService;

        public RoomController(ILogger<RoomController> logger,
            IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        [HttpGet("getRooms")]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await GetRooms();
            return Ok(rooms);
        }

        [HttpGet("getActiveRooms")]
        public async Task<ActionResult<List<Room>>> GetActiveRooms()
        {
            var rooms = await GetActiveRooms();
            return Ok(rooms);
        }
        [HttpPut("createRoom")]
        public async Task<IActionResult> CreateRoom(Room roomToCreate)
        {
            await CreateRoom(roomToCreate);
            return Ok();
        }

        [HttpPut("updateRoom")]
        public async Task<IActionResult> UpdateRoom(Room roomToUpdate)
        {
            await UpdateRoom(roomToUpdate);
            return Ok();
        }
    }
}
