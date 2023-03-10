using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRoomRepository
{
    Task Create(Room room, CancellationToken cancellationToken);

    Task Update(Room updatedRoom, CancellationToken cancellationToken);

    Task<Room?> Find(Guid id, CancellationToken cancellationToken);

    Task<Room[]> Find(bool isClosed, CancellationToken cancellationToken);
}
