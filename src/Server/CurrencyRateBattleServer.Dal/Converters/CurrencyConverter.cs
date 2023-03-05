using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using NbuClient.Dto;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class CurrencyConverter
{
    public static Currency ToDomain(this CurrencyDal dal)
    {
        return Currency.Create(dal.CurrencyName, dal.CurrencyCode, dal.Rate, dal.Description, dal.UpdateDate);
    }

    public static CurrencyDal ToDal(this Currency currency)
    {
        return new CurrencyDal
        {
            CurrencyCode = currency.CurrencySymbol.Value,
            CurrencyName = currency.CurrencyName.Value,
            Description = currency.Description,
            Rate = currency.Rate.Value,
            UpdateDate = currency.UpdateDate
        };
    }
}
