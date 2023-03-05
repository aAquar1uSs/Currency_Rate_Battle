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

    public async Task CreateAsync(Account account, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(CreateAsync)} was caused.");
        var accountDal = account.ToDal();

        await _dbContext.Accounts.AddAsync(accountDal, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"{nameof(CreateAsync)} successfully added new account");
    }

    public async Task<Account?> GetAccountByUserIdAsync(Email email, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetAccountByUserIdAsync)} was caused.");

        var account = await _dbContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(acc => acc.User.Email == email.Value, cancellationToken);

        return account?.ToDomain();
    }

    public async Task<Account?> GetAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(acc => acc.Id == accountId.Id, cancellationToken);

        return account?.ToDomain();
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        var accountDal = account.ToDal();
        
        //ToDo Do somethings with this shit || If remove this exception was thrown
        _dbContext.Accounts.Attach(accountDal);
        _dbContext.Entry(accountDal).Property(x => x.Amount).IsModified = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dbContext.Entry(accountDal).State = EntityState.Detached;
    }
}
