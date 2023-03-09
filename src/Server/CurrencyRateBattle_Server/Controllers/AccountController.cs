using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Login;
using CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Registration;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Infrastructure;
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
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync([FromBody] UserDto userData, CancellationToken cancellationToken)
    {
        var command = new LoginCommand { Email = userData.Email, Password = userData.Password};

        var response= await _mediator.Send(command, cancellationToken);

        return response.IsSuccess
            ? Ok(response.Value)
            : ToErrorResponse(response.Error);
    }

    [HttpPost("registration")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Tokens), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto userData, CancellationToken cancellationToken)
    {
        var command = new RegistrationCommand { Email = userData.Email, Password = userData.Password };

        var response = await _mediator.Send(command, cancellationToken);

        return response.IsSuccess
            ? Ok(response.Value)
            : ToErrorResponse(response.Error);
    }

    private IActionResult ToErrorResponse(Error error) => error switch
    {
        PlayerValidationError => BadRequest(error.ToDto()),
        MoneyValidationError => BadRequest(error.ToDto()),
        _ => throw new NotSupportedException($"Unknown type of error {error.GetType()}")
    };
}
