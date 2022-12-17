using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IRoomRepository
{
    Task GenerateRoomsByCurrencyCountAsync();

    Task<CurrencyState> CreateAsync(CurrencyDal curr);

    Task UpdateAsync(Guid id, RoomDal updatedRoomDal);

    Task CheckRoomsStateAsync();

    Task<Room[]> FindAsync(bool? isClosed);

    Task<RoomDal?> FindAsync(Guid id);

    Task<Room?[]> GetActiveRoomsWithFilterAsync(FilterDto filter);

    Task DeleteAsync(Guid roomId);
}
