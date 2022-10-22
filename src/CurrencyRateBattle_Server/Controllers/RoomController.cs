using System.Net;
using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetFilteredRoom;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;
using CurrencyRateBattleServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/rooms")]
[ApiController]
[Authorize]
public class RoomController : ControllerBase
{
    private readonly ILogger<RoomController> _logger;

    private readonly IMediator _mediator;

    public RoomController(ILogger<RoomController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("get-rooms/{isClosed}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<IEnumerable<Room>>> GetRoomsAsync([FromRoute] bool isClosed)
    {
        _logger.LogDebug("List of rooms are retrieving.");
        var command = new GetRoomCommand {IsClosed = isClosed};

        var response = await _mediator.Send(command);

        return Ok(response.Value.Rooms);
    }

    [HttpPost("filter")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<List<Room>>> FilterRoomsAsync([FromBody] FilterDto filter)
    {
        _logger.LogDebug("Filtered room list.");

        var command = new GetFilteredRoomCommand { Filter = filter };

        var (_, isFailure, value) = await _mediator.Send(command);

        if (isFailure)
            return BadRequest();

        return Ok(value.Rooms);
    }
}
