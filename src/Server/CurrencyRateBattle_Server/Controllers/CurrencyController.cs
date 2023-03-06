using System.Net;
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

    [HttpGet("get-currency-rates")]
    [ProducesResponseType(typeof(GetCurrencyResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetCurrencyRates(CancellationToken cancellationToken)
    {
        var command = new GetCurrencyCommand();

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value);
    }
}
