using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetRates;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.Errors;
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

    [HttpGet]
    [ProducesResponseType(typeof(GetRatesResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Room>>> GetRatesAsync([FromQuery] bool isActive, [FromQuery] string? currencyCode, CancellationToken cancellationToken)
    {
        var command = new GetRatesCommand { IsActive = isActive, CurrencyName = currencyCode };

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value);
    }

    [HttpGet("user/bets")]
    [ProducesResponseType(typeof(BetInfoDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserBetsAsync()
    {
        var userEmal = GuidHelper.GetEmailFromRequest(HttpContext);
        if (userEmal is null)
            return BadRequest();

        var command = new GetUserBetsCommand(userEmal);

        var response = await _mediator.Send(command);

        return response.IsSuccess
            ? Ok(response.Value.Bets)
            : ToErrorResponse(response.Error);
    }

    [HttpPost("make-bet")]
    [ProducesResponseType(typeof(UserRateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRateAsync([FromBody] UserRateDto userRateToCreate)
    {
        var userEmal = GuidHelper.GetEmailFromRequest(HttpContext);
        if (userEmal is null)
            return Unauthorized();

        var command = new MakeBetCommand(userEmal, userRateToCreate);

        var response = await _mediator.Send(command);

        return response.IsSuccess
            ? Ok(response.Value.UserRate)
            : ToErrorResponse(response.Error);
    }
    
    private IActionResult ToErrorResponse(Error error) => error switch
    {
        PlayerValidationError => BadRequest(error.ToDto()),
        RoomValidationError => BadRequest(error.ToDto()),
        MoneyValidationError => BadRequest(error.ToDto()),
        CommonError => BadRequest(error.ToDto()),
        RateValidationError => BadRequest(error.ToDto()),
        _ => throw new NotSupportedException($"Unknown type of error {error.GetType()}")
    };
}
