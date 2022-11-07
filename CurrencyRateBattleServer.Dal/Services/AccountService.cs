using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<IAccountService> _logger;

    private readonly CurrencyRateBattleContext _dbContext;

    public AccountService(ILogger<AccountService> logger, CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task CreateAccountAsync(Account account)
    {
        _logger.LogDebug($"{nameof(CreateAccountAsync)} was caused.");
        var accountDal = account.ToDal();

        await _dbContext.Accounts.AddAsync(accountDal);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"{nameof(CreateAccountAsync)} successfully added new account");
    }

    public async Task<Account?> FindAsync(Guid? userId)
    {
        _logger.LogDebug($"{nameof(FindAsync)} was caused.");

        var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(acc => acc.User.Id == userId);

        return account?.ToDomain();
    }
}
