using System.Net;
using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetProfile;
using CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetUserBalance;
using CurrencyRateBattleServer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/profile")]
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
    public async Task<IActionResult> GetUserBalanceAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetUserBalanceAsync)} was triggered.");

        var userId = GuidHelper.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();

        var command = new GetUserBalanceCommand { UserId = userId.Value };

        var (_, isFailure, value, error) = await _mediator.Send(command, cancellationToken);

        if (isFailure)
            return BadRequest(error);

        return Ok(value.Amount);
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetUserInfoAsync)} was triggered.");

        var userId = GuidHelper.GetGuidFromRequest(HttpContext);
        if (userId is null)
            return BadRequest();

        var command = new GetProfileCommand { UserId = userId.Value };

        var (_, isFailure, value, error) = await _mediator.Send(command, cancellationToken);

        if (isFailure)
            return BadRequest(error);

        return Ok(value.AccountInfo);
    }
}
