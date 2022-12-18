using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ILogger<IAccountRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public AccountRepository(ILogger<AccountRepository> logger, CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task CreateAccountAsync(Account account, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(CreateAccountAsync)} was caused.");
        var accountDal = account.ToDal();

        await _dbContext.Accounts.AddAsync(accountDal, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"{nameof(CreateAccountAsync)} successfully added new account");
    }

    public async Task<Account?> GetAccountByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetAccountByUserIdAsync)} was caused.");

        var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(acc => acc.User.Id == userId.Id, cancellationToken);

        return account?.ToDomain();
    }

    public async Task<Account?> GetAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId.Id, cancellationToken);

        return account?.ToDomain();
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        var accountDal = account.ToDal();

        _dbContext.Accounts.Update(accountDal);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
