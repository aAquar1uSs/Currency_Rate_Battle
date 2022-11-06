using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IRatingService
{
    Task<UserRating[]> GetUsersRatingAsync();
}
