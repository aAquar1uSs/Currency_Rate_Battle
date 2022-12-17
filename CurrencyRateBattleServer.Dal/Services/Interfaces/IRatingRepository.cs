using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IRatingRepository
{
    Task<UserRating[]> GetUsersRatingAsync();
}
