using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRoomRepository
{
    Task CreateAsync(CancellationToken cancellationToken);

    Task UpdateAsync(RoomDal updatedRoomDal, CancellationToken cancellationToken);

    Task<Room[]> RoomClosureCheckAsync(CancellationToken cancellationToken);

    Task<RoomDal?> FindAsync(Guid id, CancellationToken cancellationToken);

    Task DeleteAsync(RoomId roomId, CancellationToken cancellationToken);
}
