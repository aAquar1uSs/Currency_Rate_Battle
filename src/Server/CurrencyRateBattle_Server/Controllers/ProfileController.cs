using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetProfile;
using CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetUserBalance;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Dto;
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
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("balance")]
    [ProducesResponseType(typeof(GetUserBalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserBalanceAsync(CancellationToken cancellationToken)
    {
        var userEmal = GuidHelper.GetEmailFromRequest(HttpContext);
        if (userEmal is null)
            return Unauthorized();

        var command = new GetUserBalanceCommand { UserId = userEmal };

        var response = await _mediator.Send(command, cancellationToken);

        return response.IsSuccess
            ? Ok(response.Value)
            : ToErrorResponse(response.Error);
    }

    [HttpGet]
    [ProducesResponseType(typeof(AccountInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        var userEmail = GuidHelper.GetEmailFromRequest(HttpContext);
        if (userEmail is null)
            return Unauthorized();

        var command = new GetProfileCommand { UserEmail = userEmail };

        var response = await _mediator.Send(command, cancellationToken);

        return response.IsSuccess
            ? Ok(response.Value.AccountInfo)
            : ToErrorResponse(response.Error);  
    }
    
    private IActionResult ToErrorResponse(Error error) => error switch
    {
        PlayerValidationError => BadRequest(error.ToDto()),
        _ => throw new NotSupportedException($"Unknown type of error {error.GetType()}")
    };
}
