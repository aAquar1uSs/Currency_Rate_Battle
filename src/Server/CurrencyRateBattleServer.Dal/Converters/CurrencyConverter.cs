using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using NbuClient.Dto;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class CurrencyConverter
{
    public static CurrencyState ToDomain(this CurrencyStateDal dal)
    {
        return CurrencyState.Create(dal.Id, dal.UpdateDate, dal.CurrencyExchangeRate, dal.RoomId, dal.CurrencyCode,
            dal.CurrencyName);
    }

    public static Currency ToDomain(this CurrencyDal dal)
    {
        return Currency.Create(dal.CurrencyName, dal.CurrencyCode, dal.Rate, dal.Description, dal.UpdateDate);
    }
}
