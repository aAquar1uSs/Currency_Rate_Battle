using CurrencyRateBattleServer.Dal.Services.Interfaces;

namespace CurrencyRateBattleServer.Dal.Services;

public class RatingRepository : IRatingRepository
{
    public Task<List<UserRatingDto>> GetUsersRatingAsync()
    {
        throw new NotImplementedException();
    }
}
