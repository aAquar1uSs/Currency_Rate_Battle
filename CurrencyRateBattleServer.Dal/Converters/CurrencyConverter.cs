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
}
