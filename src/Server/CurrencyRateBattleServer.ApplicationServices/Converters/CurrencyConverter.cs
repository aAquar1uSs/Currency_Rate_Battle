using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class CurrencyConverter
{
    public static CurrencyDto ToDto(this Currency currency)
    {
        return new CurrencyDto
        {
            Currency = currency.CurrencySymbol?.Value,
            Date = DateTime.UtcNow.ToString(),
            Rate = currency.Rate.Value
        };

    }
}
