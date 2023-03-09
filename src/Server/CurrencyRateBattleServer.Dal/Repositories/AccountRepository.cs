using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly CurrencyRateBattleContext _dbContext;

    public AccountRepository(CurrencyRateBattleContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task Create(Account account, CancellationToken cancellationToken)
    {
        var accountDal = account.ToDal();

        await _dbContext.Accounts.AddAsync(accountDal, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Account?> Get(AccountId accountId, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(acc => acc.Id == accountId.Id, cancellationToken);

        return account?.ToDomain();
    }

    public async Task Update(Account account, CancellationToken cancellationToken)
    {
        var accountDal = account.ToDal();
        
        //ToDo Do somethings with this shit || If remove this exception was thrown
        _dbContext.Accounts.Attach(accountDal);
        _dbContext.Entry(accountDal).Property(x => x.Amount).IsModified = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dbContext.Entry(accountDal).State = EntityState.Detached;
    }
}
