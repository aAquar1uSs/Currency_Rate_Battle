using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class RatesConverter
{
    public static UserRateDto ToDto(this Rate rate)
    {
        return new()
        {
            Amount = rate.Amount.Value, RoomId = rate.RoomId.Id, UserCurrencyExchange = rate.RateCurrencyExchange.Value
        };
    }
}
