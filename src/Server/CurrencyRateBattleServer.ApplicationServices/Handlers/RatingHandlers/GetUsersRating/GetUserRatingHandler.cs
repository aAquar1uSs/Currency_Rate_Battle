using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RatingHandlers.GetUsersRating;

public class GetUserRatingHandler : IRequestHandler<GetUserRatingCommand, Result<GetUserRatingResponse>>
{
    private readonly IUserRatingQueryRepository _userRatingQueryRepository;

    public GetUserRatingHandler(IUserRatingQueryRepository userRatingQueryRepository)
    {
        _userRatingQueryRepository = userRatingQueryRepository ?? throw new ArgumentNullException(nameof(userRatingQueryRepository));
    }

    public async Task<Result<GetUserRatingResponse>> Handle(GetUserRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await _userRatingQueryRepository.GetUsersRating();

        return new GetUserRatingResponse
        {
            UserRating = rating.Select(x => x.ToDto()).ToArray()
        };
    }
}
