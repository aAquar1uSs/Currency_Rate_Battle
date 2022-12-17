using System.Net;
using CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/currency")]
[ApiController]
[Authorize]
public class CurrencyStateController : ControllerBase
{
    private readonly ILogger<CurrencyStateController> _logger;
    private readonly IMediator _mediator;

    public CurrencyStateController(ILogger<CurrencyStateController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetCurrencyRates(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyRates)}, was caused.");

        var command = new GetCurrencyStateCommand();

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value.CurrencyStates);
    }
}
