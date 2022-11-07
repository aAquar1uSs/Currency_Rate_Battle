using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class RateConverter
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
}
