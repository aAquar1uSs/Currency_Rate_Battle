using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class RoomConverter
{
    public static RoomDto[] ToDto(this RoomInfo[] domain)
        => domain.Select(ToDto).ToArray();
    
    public static RoomDto ToDto(this RoomInfo domain)
    {
        return new RoomDto
        {
            СurrencyName = domain.CurrencyName,
            CountRates = domain.CountRates,
            CurrencyExchangeRate = domain.CurrencyExchangeRate,
            Date = domain.Date,
            Id = domain.Id,
            IsClosed = domain.IsClosed,
            UpdateRateTime = domain.UpdateRateTime
        };
    }

    public static RoomDto ToDto(this Room room)
    {
        return new()
        {
            CountRates = room.CountRates,
            CurrencyExchangeRate = room.CountRates,
            СurrencyName = room.CurrencyName.Value,
            Date = room.EndDate,
            UpdateRateTime = DateTime.UtcNow //ToDo think about it
        };
    }
}
