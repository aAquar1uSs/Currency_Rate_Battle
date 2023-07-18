using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class RateRepository : IRateRepository
{
    private readonly CurrencyRateBattleContext _dbContext;

    public RateRepository(CurrencyRateBattleContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        
        _dbContext = dbContext;
    }

    public async Task Create(Rate rate, CancellationToken cancellationToken)
    {
        var rateDal = rate.ToDal();

        await _dbContext.Rates.AddAsync(rateDal, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRange(Rate[] updatedRate, CancellationToken cancellationToken)
    {
        var updatedRateDal = updatedRate.ToDal();

        _dbContext.Rates.UpdateRange(updatedRateDal);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Rate[]> Find(bool? isActive, string? currencyName, CancellationToken cancellationToken)
    {
        var currencyId = string.Empty;
        if (currencyName is not null)
        {
            var currency = await _dbContext.Currencies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CurrencyName == currencyName, cancellationToken);
            currencyId = currency?.CurrencyName;
        }

        //ToDo Refactor this
        RateDal[] result;
        if (currencyId is not null || currencyId != string.Empty)
            result = await _dbContext.Rates
                .Where(dal => dal.Currency.CurrencyName == currencyId)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

        result = isActive switch
        {
            null => await _dbContext.Rates.AsNoTracking().ToArrayAsync(cancellationToken),
            true => await _dbContext.Rates.Where(r => !r.IsClosed).AsNoTracking().ToArrayAsync(cancellationToken),
            _ => await _dbContext.Rates.Where(r => r.IsClosed).AsNoTracking().ToArrayAsync(cancellationToken)
        };

        return result.ToDomain();
    }

    public async Task Update(Rate rate, CancellationToken cancellationToken)
    {
        var rateDal = rate.ToDal();
        _ = _dbContext.Rates.Update(rateDal);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
