﻿using CurrencyRateBattleServer.Dal.Entities;
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
            rate.RoomId, rate.CurrencyId,
            rate.AccountId);
    }

    public static RateDal ToDal(this Rate rate)
    {
        return new RateDal
        {
            AccountId = rate.AccountId.Id,
            Amount = rate.Amount.Value,
            CurrencyId = rate.CurrencyId.Id,
            IsClosed = rate.IsClosed,
            IsWon = rate.IsWon,
            Id = rate.Id.Id,
            SetDate = rate.SetDate,
            RateCurrencyExchange = rate.RateCurrencyExchange.Value
        };
    }
}
