using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class RatesConverter
{
    public static Rate ToDomain(this RateDal rate)
    {
        return new()
        {
            Room = rate.Room.ToDomain(),
            Account = rate.Account.ToDomain(),
            Amount = rate.Amount,
            Currency = rate.Currency.ToDomain(),
            Id = rate.Id,
            IsClosed = rate.IsClosed,
            IsWon = rate.IsWon,
            Payout = rate.Payout,
            RateCurrencyExchange = rate.RateCurrencyExchange,
            SetDate = rate.SetDate,
            SettleDate = rate.SettleDate
        };
    }

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
