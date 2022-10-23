using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RatingHandlers.GetUsersRating;

public class GetUserRatingHandler : IRequestHandler<GetUserRatingCommand, Result<GetUserRatingResponse>>
{
    private readonly ILogger<GetUserRatingHandler> _logger;

    private readonly IRatingService _ratingService;

    public GetUserRatingHandler(ILogger<GetUserRatingHandler> logger, IRatingService ratingService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
    }

    public async Task<Result<GetUserRatingResponse>> Handle(GetUserRatingCommand request, CancellationToken cancellationToken)
    {
        //ToDo
        var rating = await _ratingService.GetUsersRatingAsync();

        return new GetUserRatingResponse();
    }
}
