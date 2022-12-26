using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RatingHandlers.GetUsersRating;

public class GetUserRatingHandler : IRequestHandler<GetUserRatingCommand, Result<GetUserRatingResponse>>
{
    private readonly ILogger<GetUserRatingHandler> _logger;
    private readonly IUserRatingQueryRepository _userRatingQueryRepository;

    public GetUserRatingHandler(ILogger<GetUserRatingHandler> logger, IUserRatingQueryRepository userRatingQueryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRatingQueryRepository = userRatingQueryRepository ?? throw new ArgumentNullException(nameof(userRatingQueryRepository));
    }

    public async Task<Result<GetUserRatingResponse>> Handle(GetUserRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await _userRatingQueryRepository.GetUsersRatingAsync();

        return new GetUserRatingResponse
        {
            UserRating = rating.ToDto()
        };
    }
}
