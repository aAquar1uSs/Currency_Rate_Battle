using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRoomService
{
    public Room CreateRoom(Room room);

    public void UpdateRoom(Room room);

    public List<Room> GetRooms();

    public List<Room> GetActiveRooms();

}
