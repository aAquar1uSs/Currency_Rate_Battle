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

    public async Task Create(Room room, CancellationToken cancellationToken)
    {
        var roomDal = room.ToDal();
        await _dbContext.Rooms.AddAsync(roomDal, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Update(Room updatedRoom, CancellationToken cancellationToken)
    {
        var roomDal = updatedRoom.ToDal();
        _ = _dbContext.Rooms.Update(roomDal);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Room?> Find(Guid id, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        
        return room?.ToDomain();
    }
    
    public async Task<Room[]> Find(bool isClosed, CancellationToken cancellationToken)
    {
        var rooms = await _dbContext.Rooms
            .AsNoTracking()
            .Where(dal => dal.IsClosed == isClosed)
            .Include(dal => dal.Currency)
            .ToArrayAsync(cancellationToken);
        
        return rooms.Select(dal => dal.ToDomain()).ToArray();
    }
}
