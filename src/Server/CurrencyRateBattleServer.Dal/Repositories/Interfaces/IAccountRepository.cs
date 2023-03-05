using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IAccountRepository
{
    public Task CreateAsync(Account account, CancellationToken cancellationToken);
    
    public Task<Account?> GetAccountByUserIdAsync(Email userEmail, CancellationToken cancellationToken);

    public Task<Account?> GetAsync(AccountId accountId, CancellationToken cancellationToken);

    public Task UpdateAsync(Account account, CancellationToken cancellationToken);
}
