using System.Net;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("filter")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<List<Room>>> FilterRoomsAsync([FromBody] Filter filter)
    {
        _logger.LogDebug("Filtered room list.");

        var rooms = await _roomService.GetActiveRoomsWithFilterAsync(filter);

        return rooms is null ? BadRequest() : Ok(rooms);
    }
}
