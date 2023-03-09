using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IAccountQueryRepository
{
    public Task<Account?> GetAccountByUserId(Email userEmail, CancellationToken cancellationToken);
}
