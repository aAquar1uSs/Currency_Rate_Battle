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

    public async Task<Room[]> GetActiveRoomsWithFilterAsync(Filter filter, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetActiveRoomsWithFilterAsync)} was caused");
        var rooms = await _dbContext.Rooms
            .AsNoTracking()
            .Where(dal => dal.IsClosed == false)
            .ToArrayAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(filter.CurrencyName))
            rooms = rooms.Where(room => room.CurrencyName == filter.CurrencyName.ToUpperInvariant()).ToArray();

        if (filter.DateTryParse(filter.StartDate, out var startDate))
            rooms = rooms.Where(room => room.EndDate >= startDate).ToArray();
        if (filter.DateTryParse(filter.EndDate, out var endDate))
            rooms = rooms.Where(room => room.EndDate <= endDate).ToArray();


        return rooms.Select(x => x.ToDomain()).ToArray();
    }
    
    public async Task<Room[]> RoomClosureCheckAsync(CancellationToken cancellationToken)
    {
        var closedRooms = await _dbContext.Rooms
            .AsNoTracking()
            .Where(dal => (dal.EndDate.Date == DateTime.Today
                           && dal.EndDate.Hour == DateTime.UtcNow.AddHours(1).Hour)
                          || ((dal.EndDate.Date == DateTime.Today.AddDays(1))
                              && dal.EndDate.Hour == 0 && DateTime.UtcNow.Hour == 23)
                          || DateTime.UtcNow > dal.EndDate)
            .Select(dal => new RoomDal() { EndDate = dal.EndDate, IsClosed = true, Id = dal.Id, CurrencyName = dal.CurrencyName})
            .ToArrayAsync(cancellationToken);
        _dbContext.Rooms.UpdateRange(closedRooms);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return closedRooms.ToDomain();
    }
}
