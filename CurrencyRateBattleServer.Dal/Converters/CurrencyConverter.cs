using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class CurrencyConverter
{
    public static Currency ToDomain(this CurrencyDal currencyDal)
    {
        return Currency.Create(currencyDal.Id, currencyDal.CurrencyName, currencyDal.CurrencyCode, currencyDal.Rate,
            currencyDal.Description);
    }
}
