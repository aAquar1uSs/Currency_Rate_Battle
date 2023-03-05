using System.Net;
using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAccountHistoryAsync(CancellationToken cancellationToken)
    {
        var userEmail = GuidHelper.GetGuidFromRequest(HttpContext);
        if (userEmail is null)
            return BadRequest();
        
        var command = new GetAccountHistoryCommand { UserEmail = userEmail };

        var (_, isFailure, value, error) = await _mediator.Send(command, cancellationToken);

        if (isFailure)
            return BadRequest(error);

        return Ok(value.AccountHistories);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateNewAccountHistory([FromBody] AccountHistoryDto historyDto, CancellationToken cancellationToken)
    {
        var userId = GuidHelper.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();

        var command = historyDto.ToCreateCommand(userId);

        var (_, isFailure, _, error) = await _mediator.Send(command, cancellationToken);

        if (isFailure)
            return BadRequest(error);
        
        return Ok();
    }
}
