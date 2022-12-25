using System.Net;
using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Login;
using CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Registration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IMediator mediator, ILogger<AccountController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] UserDto userData, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(LoginAsync)} was triggered.");

        var command = new LoginCommand { UserDto = userData };

        var response= await _mediator.Send(command, cancellationToken);

        if (response.IsFailure)
            return Unauthorized(response.Error);

        return Ok(response.Value.Tokens);
    }

    [HttpPost("registration")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto userData, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(CreateUserAsync)} was triggered.");

        var command = new RegistrationCommand { UserDto = userData };

        var (_, isFailure, value, error) = await _mediator.Send(command, cancellationToken);

        if (isFailure)
            return BadRequest(error);

        return Ok(value.Tokens);
    }
}
