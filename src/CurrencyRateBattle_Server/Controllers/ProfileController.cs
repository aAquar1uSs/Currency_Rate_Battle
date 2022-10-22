using System.Net;
using CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetProfile;
using CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetUserBalance;
using CurrencyRateBattleServer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("profile")]
[ApiController]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;

    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator, ILogger<ProfileController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("get-balance")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetUserBalanceAsync()
    {
        _logger.LogDebug($"{nameof(GetUserBalanceAsync)} was triggered.");

        var userId = GuidHelper.GetGuidFromRequest(HttpContext);

        var command = new GetUserBalanceCommand { UserId = userId };

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return BadRequest(response.Error);

        return Ok(response.Value.Amount);
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetUserInfoAsync()
    {
        _logger.LogDebug($"{nameof(GetUserInfoAsync)} was triggered.");

        var guidId = GuidHelper.GetGuidFromRequest(HttpContext);

        var command = new GetProfileCommand { UserId = guidId };

        var response = await _mediator.Send(command);

        if (response.IsFailure)
            return BadRequest(response.Error);
        
        return Ok(response.Value.AccountInfo);
    }
}
