using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class RateQueryRepository : IRateQueryRepository
{
    private readonly CurrencyRateBattleContext _dbContext;

    public RateQueryRepository(CurrencyRateBattleContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<Rate[]> GetActiveRateByRoomIdsAsync(IEnumerable<RoomId> roomIds, CancellationToken cancellationToken)
    {
        var roomGuidIds = roomIds.Select(x => x.Id); 
        
        var rates = await _dbContext.Rates
            .AsNoTracking()
            .Where(dal => roomGuidIds.Contains(dal.RoomId))
            .Where(dal => !dal.IsClosed)
            .ToArrayAsync(cancellationToken);

        return rates.ToDomain();
    }
}
