using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class RoomQueryRepository : IRoomQueryRepository
{
    private readonly ILogger<RoomQueryRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public RoomQueryRepository(ILogger<RoomQueryRepository> logger, CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<RoomInfo[]> FindAsync(bool isClosed, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(FindAsync)} was caused");
        
        var result = from curr in _dbContext.Currencies
            join currState in _dbContext.CurrencyStates on curr.Id equals currState.CurrencyId
            join room in _dbContext.Rooms on currState.RoomId equals room.Id
            where room.IsClosed == isClosed
            select new RoomInfoDal
            {
                Id = room.Id,
                CurrencyName = curr.CurrencyName,
                Date = room.Date,
                IsClosed = room.IsClosed,
                CurrencyExchangeRate = currState.CurrencyExchangeRate,
                UpdateRateTime = currState.Date,
                CountRates = _dbContext.Rates.Count(r => r.RoomId == room.Id)
            };
        var roomInfos = await result.ToArrayAsync(cancellationToken);
        return roomInfos.Select(x => x.ToDomain()).ToArray();
    }
    
    public async Task<RoomInfo[]> GetActiveRoomsWithFilterAsync(Filter filter, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetActiveRoomsWithFilterAsync)} was caused");
        var filteredRooms =
            await (from currencyState in _dbContext.CurrencyStates
            join room in _dbContext.Rooms on currencyState.RoomId equals room.Id
            join curr in _dbContext.Currencies on currencyState.CurrencyId equals curr.Id
            where room.IsClosed == false
            select new RoomInfoDal
            {
                Id = room.Id,
                Date = room.Date,
                CurrencyExchangeRate = currencyState.CurrencyExchangeRate,
                IsClosed = room.IsClosed,
                CurrencyName = curr.CurrencyName,
                UpdateRateTime = currencyState.Date,
                CountRates = _dbContext.Rates.Count(r => r.RoomId == room.Id)
            }).ToArrayAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(filter.CurrencyName))
            filteredRooms = filteredRooms.Where(room => room.CurrencyName == filter.CurrencyName.ToUpperInvariant()).ToArray();
        
        if (filter.DateTryParse(filter.StartDate, out var startDate))
            filteredRooms = filteredRooms.Where(room => room.Date >= startDate).ToArray();
        if (filter.DateTryParse(filter.EndDate, out var endDate))
            filteredRooms = filteredRooms.Where(room => room.Date <= endDate).ToArray();
        

        return filteredRooms.Select(x => x.ToDomain()).ToArray();
    }
}
