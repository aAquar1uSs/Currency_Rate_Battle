using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly CurrencyRateBattleContext _dbContext;

    public RoomRepository(CurrencyRateBattleContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task CreateAsync(Room room, CancellationToken cancellationToken)
    {
        var roomDal = room.ToDal();
        await _dbContext.Rooms.AddAsync(roomDal, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateAsync(Room updatedRoom, CancellationToken cancellationToken)
    {
        var roomDal = updatedRoom.ToDal();
        _ = _dbContext.Rooms.Update(roomDal);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
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

    public async Task<Room?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        
        return room?.ToDomain();
    }
    
    public async Task<Room[]> FindAsync(bool isClosed, CancellationToken cancellationToken)
    {
        var rooms = await _dbContext.Rooms
            .AsNoTracking()
            .Where(dal => dal.IsClosed == isClosed)
            .Include(dal => dal.Currency)
            .ToArrayAsync(cancellationToken);
        
        return rooms.Select(dal => dal.ToDomain()).ToArray();
    }
}
