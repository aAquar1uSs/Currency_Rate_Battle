using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRoomQueryRepository
{
    Task<Room[]> GetActiveRoomsWithFilterAsync(Filter filter, CancellationToken cancellationToken);
    
    Task<Room[]> RoomClosureCheckAsync(CancellationToken cancellationToken);
}
