using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRoomRepository
{
    Task GenerateRoomsByCurrencyCountAsync();

    Task<CurrencyState> CreateAsync(CurrencyDal curr);

    Task UpdateAsync(RoomDal updatedRoomDal, CancellationToken cancellationToken);

    Task CheckRoomsStateAsync();

    Task<RoomDal?> FindAsync(Guid id, CancellationToken cancellationToken);

    Task DeleteAsync(RoomId roomId, CancellationToken cancellationToken);
}
