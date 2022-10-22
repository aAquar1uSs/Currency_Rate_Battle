using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRoomService
{
    Task GenerateRoomsByCurrencyCountAsync();

    Task<CurrencyState> CreateRoomWithCurrencyStateAsync(CurrencyDal curr);

    Task UpdateRoomAsync(Guid id, RoomDal updatedRoomDal);

    Task CheckRoomsStateAsync();

    Task<List<RoomDto>> GetRoomsAsync(bool? isClosed);

    Task<RoomDal?> GetRoomByIdAsync(Guid id);

    Task<List<RoomDto>?> GetActiveRoomsWithFilterAsync(Filter filter);

    Task DeleteRoomByIdAsync(Guid roomId);
}
