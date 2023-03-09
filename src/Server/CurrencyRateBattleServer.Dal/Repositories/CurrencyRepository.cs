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

    public async Task Update(Currency currency, CancellationToken cancellationToken)
    {
        var currencyDal = currency.ToDal();
        _ = _dbContext.Currencies.Update(currencyDal);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Currency[]> Get(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(Get)} was caused");

        var currency = await _dbContext.Currencies
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        return currency.Select(x => x.ToDomain()).ToArray();
    }
}
