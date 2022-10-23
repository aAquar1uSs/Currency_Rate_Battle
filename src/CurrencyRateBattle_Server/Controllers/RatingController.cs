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
    private readonly ILogger<RatingController> _logger;

    private readonly IMediator _mediator;

    public RatingController(ILogger<RatingController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("get-users-rating")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUsersRatingAsync()
    {
        _logger.LogDebug($"{nameof(GetUsersRatingAsync)},  was caused.");

        var command = new GetUserRatingCommand();

        var response = await _mediator.Send(command);

        return Ok(response.Value);
    }

}
