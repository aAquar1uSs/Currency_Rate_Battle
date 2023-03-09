using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class CurrencyQueryRepository : ICurrencyQueryRepository
{
    private readonly CurrencyRateBattleContext _dbContext;

    public CurrencyQueryRepository(CurrencyRateBattleContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    public async Task<string[]> GetAllIds(CancellationToken cancellationToken)
    {
        return await _dbContext.Currencies
            .AsNoTracking()
            .Select(x => x.CurrencyName).ToArrayAsync(cancellationToken);
    }

    public async Task<decimal> GetRateByCurrencyName(string currencyName, CancellationToken cancellationToken)
    {
        var value = await _dbContext.Currencies
            .AsNoTracking()
            .Where(x => x.CurrencyName == currencyName)
            .Select(x => x.Rate)
            .FirstOrDefaultAsync(cancellationToken);

        return Math.Round(value, 2);
    }
}
