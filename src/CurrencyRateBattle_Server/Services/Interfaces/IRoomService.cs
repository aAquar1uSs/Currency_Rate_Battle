using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRoomService
{
    public Room CreateRoom(Room room);

    public void UpdateRoom(Guid id, Room updatedRoom);

    public Task<List<Room>> GetRoomsAsync(bool isActive);

    public Room? GetRoomById(Guid id);

}
