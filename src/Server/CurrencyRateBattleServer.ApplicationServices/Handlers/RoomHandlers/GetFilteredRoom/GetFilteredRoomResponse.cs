using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetFilteredRoom;

public class GetFilteredRoomResponse
{
    public RoomDto[] Rooms { get; set; }
}
