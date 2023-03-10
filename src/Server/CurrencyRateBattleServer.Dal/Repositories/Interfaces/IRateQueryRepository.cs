using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRateQueryRepository
{
    Task<Rate[]> GetActiveRateByRoomIdsAsync(RoomId[] roomIds, CancellationToken cancellationToken);
}
