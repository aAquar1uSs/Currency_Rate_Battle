using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IUserRatingQueryRepository
{
    Task<UserRating[]> GetUsersRatingAsync();

    Task<Bet[]> FindAsync(AccountId accountId, CancellationToken cancellationToken);
}
