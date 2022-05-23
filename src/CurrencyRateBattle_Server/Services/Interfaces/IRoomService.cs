using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRoomService
{
    public Task CreateRoomAsync(CurrencyRateBattleContext db, Currency curr);

    public void UpdateRoomAsync(Guid id, Room updatedRoom);

    public Task<List<RoomDto>> GetRoomsAsync(bool? isActive);

    public Task<Room?> GetRoomByIdAsync(Guid id);

    public Task<List<RoomDto>?> GetActiveRoomsWithFilterByCurrencyNameAsync(string currencyName);
}
