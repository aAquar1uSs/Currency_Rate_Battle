using System.Net;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetRates;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/rates")]
[ApiController]
[Authorize]
public class RateController : ControllerBase
{
    private readonly IMediator _mediator;

    public RateController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet] //ToDo add isActive and CurrencyCode in DTO
    public async Task<ActionResult<List<Room>>> GetRatesAsync(bool? isActive, string? currencyCode, CancellationToken cancellationToken)
    {
        var command = new GetRatesCommand {IsActive = isActive, CurrencyName = currencyCode};

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value.Rates);
    }

    [HttpGet("get-user-bets")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUserBetsAsync()
    {
        var userId = GuidHelper.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();

        var command = new GetUserBetsCommand {UserId = (Guid)userId};

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return BadRequest(response.Error);

        return Ok(response.Value.Bets);
    }

    [HttpPost("make-bet")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateRateAsync([FromBody] UserRateDto userRateToCreate)
    {
        var userId = GuidHelper.GetGuidFromRequest(HttpContext);

        if (userId is null)
            return BadRequest("Incorrect data");

        var command = new MakeBetCommand { UserRateToCreate = userRateToCreate, UserId = (Guid)userId };

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return BadRequest(response.Error);

        return Ok(response.Value.UserRate);
    }

}
