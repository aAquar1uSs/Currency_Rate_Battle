using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class RateRepository : IRateRepository
{
    private readonly ILogger<RateRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public RateRepository(ILogger<RateRepository> logger, CurrencyRateBattleContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(dbContext);

        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Rate rate, CancellationToken cancellationToken)
    {
        var rateDal = rate.ToDal();

        await _dbContext.Rates.AddAsync(rateDal, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Rate[]> GetActiveRateByRoomIdsAsync(RoomId[] roomIds, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetActiveRateByRoomIdsAsync)} was caused.");

        var roomGuidIds = roomIds.Select(x => x.Id); 

        var rates = await _dbContext.Rates
            .Where(dal => roomGuidIds.Contains(dal.RoomId))
            .Where(dal => !dal.IsClosed)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        return rates.ToDomain();
    }

    public async Task UpdateRangeAsync(Rate[] updatedRate, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(UpdateRangeAsync)} was caused.");

        var updatedRateDal = updatedRate.ToDal();

        _dbContext.Rates.UpdateRange(updatedRateDal);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Rate[]> FindAsync(bool? isActive, string? currencyName, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(FindAsync)} was caused.");

        var currencyId = string.Empty;
        if (currencyName is not null)
        {
            var currency = await _dbContext.Currencies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CurrencyName == currencyName, cancellationToken);
            currencyId = currency?.CurrencyName;
        }

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

    public async Task DeleteRateAsync(Rate rateToDelete)
    {
        _logger.LogInformation($"{nameof(DeleteRateAsync)} was caused.");

        _ = _dbContext.Rates.Remove(rateToDelete.ToDal());
        _ = await _dbContext.SaveChangesAsync();
    }
}
