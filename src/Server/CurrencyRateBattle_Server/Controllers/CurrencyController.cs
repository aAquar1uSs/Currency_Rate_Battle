using System.Net;
using CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/currency")]
[ApiController]
[Authorize]
public class CurrencyController : ControllerBase
{
    private readonly ILogger<CurrencyController> _logger;
    private readonly IMediator _mediator;

    public CurrencyController(ILogger<CurrencyController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("get-currency-rates")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetCurrencyRates(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyRates)}, was caused.");

        var command = new GetCurrencyCommand();

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value.CurrencyStates);
    }
}
