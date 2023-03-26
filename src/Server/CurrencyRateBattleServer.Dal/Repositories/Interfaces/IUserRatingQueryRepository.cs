using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IUserRatingQueryRepository
{
    Task<UserRating[]> GetUsersRating();

    Task<Bet[]> Find(AccountId accountId, CancellationToken cancellationToken);
}
