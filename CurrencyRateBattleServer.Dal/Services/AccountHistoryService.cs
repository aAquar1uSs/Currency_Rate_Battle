using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class AccountHistoryService : IAccountHistoryService
{
    private readonly ILogger<AccountHistoryService> _logger;

    private readonly CurrencyRateBattleContext _dbContext;

    public AccountHistoryService(ILogger<AccountHistoryService> logger,
        CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<AccountHistory[]> GetAccountHistoryByAccountId(Guid? id)
    {
        _logger.LogDebug($"{nameof(GetAccountHistoryByAccountId)} was caused.");

        var histories = await _dbContext.AccountHistory
            .Where(history => history.Account.Id == id)
            .ToArrayAsync();

        return histories.ToDomain();
    }

    public async Task CreateHistoryAsync(AccountHistory accountHistory)
    {
        var historyDal = accountHistory.ToDal();

        _ = await _dbContext.AccountHistory.AddAsync(historyDal);
        _ = await _dbContext.SaveChangesAsync();

        _logger.LogInformation("New history record added to the database.");
    }
}
