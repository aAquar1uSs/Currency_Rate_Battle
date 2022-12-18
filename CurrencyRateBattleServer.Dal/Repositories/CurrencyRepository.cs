using CurrencyRateBattleServer.Dal.Entities;
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

    public async Task UpdateAsync(Currency[] currencies, CancellationToken cancellationToken)
    {
        foreach (var currency in currencies)
        {
            var currencyToUpdate = await _dbContext.Currencies
                .Where(x => x.CurrencyCode == currency.CurrencyCode.Value)
                .Select(x => new CurrencyDal()
                {
                    CurrencyCode = x.CurrencyCode,
                    CurrencyName = x.CurrencyName,
                    Description = x.Description,
                    Rate = currency.Rate.Value
                }).ToArrayAsync(cancellationToken);

            _dbContext.UpdateRange(currencyToUpdate, cancellationToken);
        }
    }
}
