using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRoomService
{
    public Task<Room> CreateRoomAsync(Room room);

    public void UpdateRoomAsync(Guid id, Room updatedRoom);

    public Task<List<Room>> GetRoomsAsync(bool? isActive);

    public Task<Room?> GetRoomByIdAsync(Guid id);

    public Task<List<RoomDto>?> GetActiveRoomsWithFilterAsync(string currencyName);
}
