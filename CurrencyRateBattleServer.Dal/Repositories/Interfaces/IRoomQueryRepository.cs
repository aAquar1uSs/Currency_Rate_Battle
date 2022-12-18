using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRoomQueryRepository
{
    Task<RoomInfo[]> FindAsync(bool isClosed, CancellationToken cancellationToken);

    Task<RoomInfo[]> GetActiveRoomsWithFilterAsync(Filter filter, CancellationToken cancellationToken);
}
