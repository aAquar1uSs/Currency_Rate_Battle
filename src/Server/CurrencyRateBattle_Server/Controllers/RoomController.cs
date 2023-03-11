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
    private readonly IMediator _mediator;

    public RoomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(RoomDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Room>>> GetRoomsAsync([FromQuery] bool isClosed, CancellationToken cancellationToken)
    {
        var command = new GetRoomCommand(isClosed);

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value.Rooms);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(RoomDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<Room>>> FilterRoomsAsync([FromBody] FilterDto filter, CancellationToken cancellationToken)
    {
        var command = new GetFilteredRoomCommand { Filter = filter };

        var response = await _mediator.Send(command, cancellationToken);
        
        return Ok(response.Value.Rooms);
    }
}
