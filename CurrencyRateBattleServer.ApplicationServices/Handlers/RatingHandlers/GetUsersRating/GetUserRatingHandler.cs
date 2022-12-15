using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RatingHandlers.GetUsersRating;

public class GetUserRatingHandler : IRequestHandler<GetUserRatingCommand, Result<GetUserRatingResponse>>
{
    private readonly ILogger<GetUserRatingHandler> _logger;

    private readonly IRatingRepository _ratingRepository;

    public GetUserRatingHandler(ILogger<GetUserRatingHandler> logger, IRatingRepository ratingRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _ratingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(ratingRepository));
    }

    public async Task<Result<GetUserRatingResponse>> Handle(GetUserRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await _ratingRepository.GetUsersRatingAsync();
        
        return new GetUserRatingResponse
        {
            UserRating = rating.ToDto()
        };
    }
}
