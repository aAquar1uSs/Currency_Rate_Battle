using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRoomService
{
    Task GenerateRoomsByCurrencyCountAsync();

    Task<CurrencyState> CreateRoomWithCurrencyStateAsync(CurrencyDal curr);

    Task UpdateRoomAsync(Guid id, RoomDal updatedRoomDal);

    Task CheckRoomsStateAsync();

    Task<Room[]> GetRoomsAsync(bool? isClosed);

    Task<RoomDal?> GetRoomByIdAsync(Guid id);

    Task<Room?[]> GetActiveRoomsWithFilterAsync(FilterDto filter);

    Task DeleteRoomByIdAsync(Guid roomId);
}
