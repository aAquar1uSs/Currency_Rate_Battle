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

    public static Currency? ToDomain(this NbuClient.Dto.CurrencyDto currencyDto)
    {
        if (currencyDto.Currency is null || currencyDto.Date is null)
            return null;

        if (!DateTime.TryParse(currencyDto.Date, out var updateDate))
            return null;

        return Currency.Create(currencyDto.Currency, currencyDto.Rate, updateDate);
    }
}
