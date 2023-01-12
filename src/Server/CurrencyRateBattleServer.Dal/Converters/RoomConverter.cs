using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class RoomConverter
{
    public static Room[] ToDomain(this RoomDal[] rooms)
    {
        return rooms.Select(x => x.ToDomain()).ToArray();
    }

    public static Room ToDomain(this RoomDal roomDal)
    {
        return Room.Create(roomDal.Id, roomDal.EndDate, roomDal.IsClosed);
    }

    public static RoomInfo ToDomain(this RoomInfoDal dal)
    {
        return new()
        {
            Id = dal.Id,
            CurrencyName = dal.CurrencyName,
            CountRates = dal.CountRates,
            CurrencyExchangeRate = dal.CurrencyExchangeRate,
            Date = dal.Date,
            IsClosed = dal.IsClosed,
            UpdateRateTime = dal.UpdateRateTime
        };
    }
}
