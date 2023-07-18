using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/history")]
[ApiController]
[Authorize]
public class AccountHistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountHistoryController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(AccountHistoryDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAccountHistoryAsync(CancellationToken cancellationToken)
    {
        var userEmail = GuidHelper.GetEmailFromRequest(HttpContext);
        if (userEmail is null)
            return Unauthorized();

        var command = new GetAccountHistoryCommand(userEmail);

        var response = await _mediator.Send(command, cancellationToken);

        return response.IsSuccess
            ? Ok(response.Value.AccountHistories)
            : ToErrorResponse(response.Error);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewAccountHistory([FromBody] AccountHistoryDto historyDto, CancellationToken cancellationToken)
    {
        var userId = GuidHelper.GetEmailFromRequest(HttpContext);
        if (userId is null)
            return Unauthorized();

        var command = historyDto.ToCreateCommand(userId);

        var response = await _mediator.Send(command, cancellationToken);

        return response.HasValue
            ? ToErrorResponse(response.Value)
            : Ok();
    }
    
    private IActionResult ToErrorResponse(Error error) => error switch
    {
        PlayerValidationError => BadRequest(error.ToDto()),
        RoomValidationError => BadRequest(error.ToDto()),
        _ => throw new NotSupportedException($"Unknown type of error {error.GetType()}")
    };
}
