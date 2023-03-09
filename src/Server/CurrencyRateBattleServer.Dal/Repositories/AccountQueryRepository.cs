using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class AccountQueryRepository : IAccountQueryRepository
{
    private readonly CurrencyRateBattleContext _dbContext;
    
    public AccountQueryRepository(CurrencyRateBattleContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Account?> GetAccountByUserId(Email userEmail, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(acc => acc.User.Email == userEmail.Value, cancellationToken);

        return account?.ToDomain();
    }
}
