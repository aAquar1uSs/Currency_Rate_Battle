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

    public async Task<Rate[]> GetRateByRoomIdAsync(RoomId[] roomIds, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetRateByRoomIdAsync)} was caused.");

        var rates = await _dbContext.Rates
            .Where(dal => roomIds.Any(x => x.Id == dal.RoomId))
            .ToArrayAsync(cancellationToken);

        return rates.ToDomain();
    }

    public async Task UpdateRateByRoomIdAsync(Guid id, RateDal updatedRateDal)
    {
        _logger.LogInformation($"{nameof(UpdateRateByRoomIdAsync)} was caused.");

        //ToDo Move this in handler
        var rateExists = await _dbContext.Rooms.AnyAsync(r => r.Id == id);
        if (!rateExists)
            throw new GeneralException($"{nameof(RateDal)} with Id={id} is not found.");

        updatedRateDal.SettleDate = DateTime.UtcNow;

        _ = _dbContext.Rates.Update(updatedRateDal);
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task<Rate[]> FindAsync(bool? isActive, string? currencyCode, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(FindAsync)} was caused.");

        var currencyId = string.Empty;
        if (currencyCode is not null)
        {
            var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.CurrencyName == currencyCode, cancellationToken);
            currencyId = currency?.CurrencyCode;
        }

        RateDal[] result;
        if (currencyId is not null || currencyId != string.Empty)
            result = await _dbContext.Rates.Where(dal => dal.Currency.CurrencyCode == currencyId).ToArrayAsync(cancellationToken);

        result = isActive switch
        {
            null => await _dbContext.Rates.ToArrayAsync(cancellationToken),
            true => await _dbContext.Rates.Where(r => !r.IsClosed).ToArrayAsync(cancellationToken),
            _ => await _dbContext.Rates.Where(r => r.IsClosed).ToArrayAsync(cancellationToken)
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
