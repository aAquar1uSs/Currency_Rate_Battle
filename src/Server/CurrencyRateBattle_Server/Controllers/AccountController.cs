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

    public AccountController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] UserDto userData, CancellationToken cancellationToken)
    {
        var command = new LoginCommand { UserDto = userData };

        var response= await _mediator.Send(command, cancellationToken);

        if (response.IsFailure)
            return Unauthorized(response.Error);

        return Ok(response.Value.Tokens.Token);
    }

    [HttpPost("registration")]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto userData, CancellationToken cancellationToken)
    {
        var command = new RegistrationCommand { UserDto = userData };

        var (_, isFailure, value, error) = await _mediator.Send(command, cancellationToken);

        if (isFailure)
            return BadRequest(error);

        return Ok(value.Tokens.Token);
    }
}
