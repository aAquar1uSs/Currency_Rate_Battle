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
             var entity = await _dbContext.Currencies
                .Where(x => x.CurrencyName == currency.CurrencyName.Value)
                .Select(x => new CurrencyDal()
                {
                    CurrencyCode = x.CurrencyCode,
                    CurrencyName = x.CurrencyName,
                    Description = x.Description,
                    Rate = currency.Rate.Value,
                    UpdateDate = x.UpdateDate
                }).FirstOrDefaultAsync(cancellationToken);

             if (entity is not null)
             {
                 _dbContext.Update(entity);
             }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<string[]> GetAllIds(CancellationToken cancellationToken)
    {
        return await _dbContext.Currencies.Select(x => x.CurrencyName).ToArrayAsync(cancellationToken);
    }
}
