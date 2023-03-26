using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyHandlers.GetCurrencyRates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/currency")]
[ApiController]
[Authorize]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("rates")]
    [ProducesResponseType(typeof(CurrencyDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrencyRates(CancellationToken cancellationToken)
    {
        var command = new GetCurrencyCommand();

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value.CurrencyStates);
    }
}
