using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRoomRepository
{
    Task CreateAsync(Room room, CancellationToken cancellationToken);

    Task UpdateAsync(Room updatedRoom, CancellationToken cancellationToken);

    Task<Room[]> RoomClosureCheckAsync(CancellationToken cancellationToken);

    Task<Room?> FindAsync(Guid id, CancellationToken cancellationToken);

    Task<Room[]> FindAsync(bool isClosed, CancellationToken cancellationToken);
}
