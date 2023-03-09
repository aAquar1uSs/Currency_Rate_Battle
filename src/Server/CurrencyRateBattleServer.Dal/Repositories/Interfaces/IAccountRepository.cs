using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IAccountRepository
{
    public Task Create(Account account, CancellationToken cancellationToken);

    public Task<Account?> Get(AccountId accountId, CancellationToken cancellationToken);

    public Task Update(Account account, CancellationToken cancellationToken);
}
