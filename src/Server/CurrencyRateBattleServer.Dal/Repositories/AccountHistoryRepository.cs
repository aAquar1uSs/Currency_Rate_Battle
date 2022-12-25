using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class AccountHistoryRepository : IAccountHistoryRepository
{
    private readonly ILogger<AccountHistoryRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public AccountHistoryRepository(ILogger<AccountHistoryRepository> logger,
        CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<AccountHistory[]> GetAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetAsync)} was caused.");

        var histories = await _dbContext.AccountHistory
            .Where(history => history.Account.Id == accountId.Id)
            .ToArrayAsync(cancellationToken);

        return histories.ToDomain();
    }

    public async Task CreateAsync(AccountHistory accountHistory, CancellationToken cancellationToken)
    {
        var historyDal = accountHistory.ToDal();

        _ = await _dbContext.AccountHistory.AddAsync(historyDal, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("New history record added to the database.");
    }
}
