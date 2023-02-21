using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly ILogger<CurrencyRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public CurrencyRepository(ILogger<CurrencyRepository> logger, CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task UpdateAsync(Currency currency, CancellationToken cancellationToken)
    {
        var currencyDal = currency.ToDal();
        _ = _dbContext.Currencies.Update(currencyDal);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<string[]> GetAllIds(CancellationToken cancellationToken)
    {
        return await _dbContext.Currencies
            .AsNoTracking()
            .Select(x => x.CurrencyName).ToArrayAsync(cancellationToken);
    }

    public async Task<decimal> GetRateByCurrencyName(string currencyName, CancellationToken cancellationToken)
    {
        var value = await _dbContext.Currencies.AsNoTracking()
            .Where(x => x.CurrencyName == currencyName)
            .Select(x => x.Rate)
            .FirstOrDefaultAsync(cancellationToken);

        return Math.Round(value, 2);
    }
    
    public async Task<Currency[]> GetAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetAsync)} was caused");

        var currency = await _dbContext.Currencies
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        return currency.Select(x => x.ToDomain()).ToArray();
    }
}
