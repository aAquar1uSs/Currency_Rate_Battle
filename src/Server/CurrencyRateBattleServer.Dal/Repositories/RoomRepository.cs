using System.Globalization;
using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ILogger<RoomRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public RoomRepository(ILogger<RoomRepository> logger, CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    //Add Settings to count of rooms
    public async Task CreateAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateAsync)} was caused");

        foreach (var curr in _dbContext.Currencies)
        {
            _ = await _dbContext.CurrencyStates.AddAsync(await CreateRoomWithCurrencyStateAsync(curr), cancellationToken);
        }
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }


    private Task<CurrencyStateDal> CreateRoomWithCurrencyStateAsync(CurrencyDal curr)
    {
        _logger.LogInformation($"{nameof(CreateRoomWithCurrencyStateAsync)} was caused");
        var currentDate = DateTime.ParseExact(
            DateTime.UtcNow.ToString("MM.dd.yyyy HH:00:00", CultureInfo.InvariantCulture),
            "MM.dd.yyyy HH:mm:ss", null);

        return Task.FromResult(new CurrencyStateDal
        {
            UpdateDate = currentDate,
            CurrencyExchangeRate = 0,
            CurrencyName = curr.CurrencyName,
            CurrencyCode = curr.CurrencyCode,
            Room = new RoomDal { EndDate = currentDate.AddDays(1), IsClosed = false }
        });
    }

    public async Task UpdateAsync(Room updatedRoom, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(UpdateAsync)} was caused");

        var roomDal = updatedRoom.ToDal();
        _ = _dbContext.Rooms.Update(roomDal);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Room[]> RoomClosureCheckAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(RoomClosureCheckAsync)} was caused");
        var closedRooms = await _dbContext.Rooms
            .Where(dal => (dal.EndDate.Date == DateTime.Today
                           && dal.EndDate.Hour == DateTime.UtcNow.AddHours(1).Hour)
                          || ((dal.EndDate.Date == DateTime.Today.AddDays(1))
                              && dal.EndDate.Hour == 0 && DateTime.UtcNow.Hour == 23)
                          || DateTime.UtcNow > dal.EndDate)
            .Select(dal => new RoomDal() { EndDate = dal.EndDate, IsClosed = true, Id = dal.Id })
            .ToArrayAsync(cancellationToken);
        _dbContext.Rooms.UpdateRange(closedRooms);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return closedRooms.ToDomain();
    }

    public async Task<Room?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(FindAsync)} was caused");
        var room = await _dbContext.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        
        return room.ToDomain();
    }
    
    public async Task<Room[]> FindAsync(bool isClosed, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(FindAsync)} was caused");
        
        var rooms = await _dbContext.Rooms
            .Where(dal => dal.IsClosed == isClosed)
            .Include(dal => dal.Currency)
            .ToArrayAsync(cancellationToken);
        
        return rooms.Select(dal => dal.ToDomain()).ToArray();
    }
}
