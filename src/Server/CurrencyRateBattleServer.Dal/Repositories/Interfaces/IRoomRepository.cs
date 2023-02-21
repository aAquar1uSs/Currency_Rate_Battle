using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRoomRepository
{
    Task CreateAsync(CancellationToken cancellationToken);

    Task UpdateAsync(Room updatedRoom, CancellationToken cancellationToken);

    Task<Room[]> RoomClosureCheckAsync(CancellationToken cancellationToken);

    Task<Room?> FindAsync(Guid id, CancellationToken cancellationToken);

    Task<Room[]> FindAsync(bool isClosed, CancellationToken cancellationToken);
}
