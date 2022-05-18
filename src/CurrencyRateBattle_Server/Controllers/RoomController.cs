using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Controllers
{
    [Route("api/rooms")]
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

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetRoomsAsync(bool? isActive)
        {
            _logger.LogDebug("List of rooms are retrieving.");
            var rooms = await _roomService.GetRoomsAsync(isActive);
            return Ok(rooms);
        }

        // GET api/Room/{id}
        [HttpGet("{id}")]
        public async Task<Room> GetRoomByIdAsync(Guid id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            return room;
        }


        [HttpPost]
        public async Task<IActionResult> CreateRoomAsync([FromBody] Room roomToCreate)
        {
            _logger.LogDebug("New room creation is trigerred.");
            try
            {
                var room = await _roomService.CreateRoomAsync(roomToCreate);
                return Ok(room);
            }
            catch (CustomException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                _logger.LogDebug("An unexpected error occurred during the attempt to create a room in the DB.");
                return BadRequest("An unexpected error occurred. Please try again.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomAsync(Guid id, [FromBody] Room updatedRoom)
        {
            try
            {
                _roomService.UpdateRoomAsync(id, updatedRoom);
                return Ok();
            }
            catch (CustomException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                _logger.LogDebug("An unexpected error occurred during the attempt to update the room in the DB.");
                return BadRequest("An unexpected error occurred. Please try again.");
            }

        }
    }
}
