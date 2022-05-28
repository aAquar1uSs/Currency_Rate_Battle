using System.Net;
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<IEnumerable<Room>>> GetRoomsAsync([FromRoute] bool isClosed)
    {
        _logger.LogDebug("List of rooms are retrieving.");
        var rooms = await _roomService.GetRoomsAsync(isClosed);
        
        return Ok(rooms);
    }

    // GET api/rooms/{id}
    [HttpGet("{id}")]
    public async Task<Room?> GetRoomByIdAsync(Guid id)
    {
        var room = await _roomService.GetRoomByIdAsync(id);
        return room;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoomAsync(Guid id, [FromBody] Room updatedRoom)
    {
        try
        {
            await _roomService.UpdateRoomAsync(id, updatedRoom);
            _logger.LogInformation($"Room has been updated successfully ({id})");
            return Ok();
        }
        catch (GeneralException ex)
        {
            return BadRequest(new {message = ex.Message});
        }
        catch (DbUpdateException)
        {
            _logger.LogDebug("An unexpected error occurred during the attempt to update the room in the DB.");
            return BadRequest("An unexpected error occurred. Please try again.");
        }
    }

    [HttpPost("filter")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<List<Room>>> FilterRoomsAsync([FromBody] Filter filter)
    {
        _logger.LogDebug("Filtered room list.");

        var rooms = await _roomService.GetActiveRoomsWithFilterAsync(filter);

        return rooms is null ? BadRequest() : Ok(rooms);
    }
}
