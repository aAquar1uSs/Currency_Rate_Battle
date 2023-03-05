using System.Net;
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

    [HttpGet("get-users-rating")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUsersRatingAsync(CancellationToken cancellationToken)
    {
        var command = new GetUserRatingCommand();

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response.Value.UserRating);
    }

}
