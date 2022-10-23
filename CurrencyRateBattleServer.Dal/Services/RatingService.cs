using CurrencyRateBattleServer.Dal.Services.Interfaces;

namespace CurrencyRateBattleServer.Dal.Services;

public class RatingService : IRatingService
{
    public Task<List<UserRatingDto>> GetUsersRatingAsync()
    {
        throw new NotImplementedException();
    }
}
