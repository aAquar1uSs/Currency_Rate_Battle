using CurrencyRateBattleServer.Dal.Converters;
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
}
