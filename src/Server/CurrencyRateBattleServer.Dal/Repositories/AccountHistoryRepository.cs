using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class AccountHistoryRepository : IAccountHistoryRepository
{
    private readonly CurrencyRateBattleContext _dbContext;

    public AccountHistoryRepository(CurrencyRateBattleContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<AccountHistory[]> Get(AccountId accountId, CancellationToken cancellationToken)
    {
        var histories = await _dbContext.AccountHistory
            .AsNoTracking()
            .Where(history => history.Account.Id == accountId.Id)
            .ToArrayAsync(cancellationToken);

        return histories.ToDomain();
    }

    public async Task Create(AccountHistory accountHistory, CancellationToken cancellationToken)
    {
        var historyDal = accountHistory.ToDal();

        _ = await _dbContext.AccountHistory.AddAsync(historyDal, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
