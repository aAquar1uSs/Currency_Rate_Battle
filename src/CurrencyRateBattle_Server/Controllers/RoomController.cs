using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Controllers;

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

    [HttpGet("get-rooms/{isClosed}")]
    public async Task<ActionResult<IEnumerable<Room>>> GetRoomsAsync([FromRoute] bool isClosed)
    {
        _logger.LogDebug("List of rooms are retrieving.");
        var rooms = await _roomService.GetRoomsAsync(isClosed);
        return Ok(rooms);
    }

    // GET api/rooms/{id}
    [HttpGet("{id}")]
    public async Task<Room> GetRoomByIdAsync(Guid id)
    {
        var room = await _roomService.GetRoomByIdAsync(id);
        return room;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoomAsync(Guid id, [FromBody] Room updatedRoom)
    {
        try
        {
            _roomService.UpdateRoomAsync(id, updatedRoom);
            _logger.LogInformation($"Room has been updated successfully ({id})");
            return Ok();
        }
        catch (CustomException ex)
        {
            // return error message if there was an exception
            return BadRequest(new {message = ex.Message});
        }
        catch (DbUpdateException)
        {
            _logger.LogDebug("An unexpected error occurred during the attempt to update the room in the DB.");
            return BadRequest("An unexpected error occurred. Please try again.");
        }
    }

    [HttpGet("filter/{currencyName}")]
    public async Task<ActionResult<List<Room>>> FilterRoomsAsync([FromRoute] string currencyName)
    {
        _logger.LogDebug("Filtered by currency room list.");

        var rooms = await _roomService.GetActiveRoomsWithFilterByCurrencyNameAsync(currencyName);

        return rooms is null ? BadRequest() : Ok(rooms);
    }
}
