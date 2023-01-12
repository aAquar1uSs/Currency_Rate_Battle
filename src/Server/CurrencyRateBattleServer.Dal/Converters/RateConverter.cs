using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class RateConverter
{
    public static Rate[] ToDomain(this RateDal[] dals) 
        => dals.Select(ToDomain).ToArray();
    
    public static Rate ToDomain(this RateDal rate)
    {
        return Rate.Create(rate.Id, rate.SetDate,
            rate.RateCurrencyExchange, rate.Amount,
            rate.SettleDate, rate.Payout,
            rate.IsClosed, rate.IsWon,
            rate.RoomId, rate.CurrencyName,
            rate.AccountId);
    }

    public static RateDal[] ToDal(this Rate[] domain)
    {
        return domain.Select(x => x.ToDal()).ToArray();
    }

    public static RateDal ToDal(this Rate rate)
    {
        return new RateDal
        {
            AccountId = rate.AccountId.Id,
            Amount = rate.Amount.Value,
            CurrencyName = rate.CurrencyName.Value,
            Payout = rate.Payout?.Value,
            IsClosed = rate.IsClosed,
            IsWon = rate.IsWon,
            Id = rate.Id.Id,
            SetDate = rate.SetDate,
            SettleDate = rate.SettleDate,
            RateCurrencyExchange = rate.RateCurrencyExchange.Value,
            RoomId = rate.RoomId.Id
        };
    }
}
