using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RatingHandlers.GetUsersRating;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRateBattleServer.Controllers;

[Route("api/rating")]
[ApiController]
[Authorize]
public class RatingController : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("users")]
    [ProducesResponseType(typeof(UserRateDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUsersRatingAsync(CancellationToken cancellationToken)
    {
        var command = new GetUserRatingCommand();

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value.UserRating);
    }

}
