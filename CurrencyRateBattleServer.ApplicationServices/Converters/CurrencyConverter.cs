using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class CurrencyConverter
{
    public static Currency ToDomain(this CurrencyDal currencyDal)
    {
        return new Currency
        {
            CurrencyName = currencyDal.CurrencyName,
            CurrencySymbol = currencyDal.CurrencyCode,
            Description = currencyDal.Description
        };
    }
}
