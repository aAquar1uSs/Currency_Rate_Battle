using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class RoomConverter
{
    public static Room[] ToDomain(this RoomDal[] rooms)
    {
        return rooms.Select(x => x.ToDomain()).ToArray();
    }

    public static Room ToDomain(this RoomDal room)
    {
        return new Room {Date = room.Date, Id = room.Id, IsClosed = room.IsClosed};
    }

    public static RoomDal ToDal(this Room room)
    {
        return new RoomDal {Date = room.Date, Id = room.Id, IsClosed = room.IsClosed};
    }
}
