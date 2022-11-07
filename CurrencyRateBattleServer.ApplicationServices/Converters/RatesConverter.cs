using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class RatesConverter
{
    public static RateDto ToDto(this Rate rate)
    {
        return new()
        {
            Amount = rate.Amount, RoomId = rate.Room.Id, UserCurrencyExchange = rate.RateCurrencyExchange,
        };
    }

    public static RateDto[] ToDto(this Rate[] rates)
    {
        return rates.Select(x => x.ToDto()).ToArray();
    }
    
}
