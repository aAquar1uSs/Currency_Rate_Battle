using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using NbuClient.Dto;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class CurrencyConverter
{
    public static Currency ToDomain(this CurrencyDal currencyDal)
    {
        return Currency.Create(currencyDal.CurrencyName, currencyDal.CurrencyCode, currencyDal.Rate,
            currencyDal.Description);
    }

    public static CurrencyDal ToDal(this CurrencyDto dto)
    {
        return new() { CurrencyCode = dto.Currency, Rate = dto.Rate };
    }

    public static CurrencyState ToDomain(this CurrencyStateDal dal)
    {
        return CurrencyState.Create(dal.Id, dal.UpdateDate, dal.CurrencyExchangeRate, dal.RoomId, dal.CurrencyCode,
            dal.CurrencyName);
    }
}
