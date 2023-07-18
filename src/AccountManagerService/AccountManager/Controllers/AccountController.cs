using AccountManager.ApplicationServices.Handlers.Login;
using AccountManager.ApplicationServices.Handlers.Registration;
using AccountManager.Converters;
using AccountManager.Domain.Errors;
using AccountManager.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ISender _sender;

    public AccountController(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync([FromBody] UserDto userData, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(userData.Email, userData.Password);

        var response = await _sender.Send(command, cancellationToken);
        
        return response.IsSuccess
            ? Ok(response.Value)
            : ToErrorResponse(response.Error);
    }

    [HttpPost("registration")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokensDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto userData, CancellationToken cancellationToken)
    {
        var command = new RegistrationCommand (userData.Email, userData.Password);

        var response = await _sender.Send(command, cancellationToken);

        return response.IsSuccess
            ? Ok(response.Value)
            : ToErrorResponse(response.Error);
    }

    private IActionResult ToErrorResponse(Error error) => error switch
    {
        UserValidationError => BadRequest(error.ToDto()),
        //MoneyValidationError => BadRequest(error.ToDto()),
        _ => throw new NotSupportedException($"Unknown type of error {error.GetType()}")
    };
}