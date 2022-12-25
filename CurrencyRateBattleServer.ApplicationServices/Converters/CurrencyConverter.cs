using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class CurrencyConverter
{
    public static CurrencyDto[] ToDto(this Currency[] currencyStates)
    {
        return currencyStates.Select(x => x.ToDto()).ToArray();
    }

    private static CurrencyDto ToDto(this Currency currency)
    {
        return new CurrencyDto
        {
            Currency = currency.CurrencyCode?.Value,
            Date = DateTime.UtcNow.ToString(),
            Rate = currency.Rate.Value
        };

    }

    public static Currency ToDomain(this NbuClient.Dto.CurrencyDto currencyDto)
    {
        return Currency.Create(currencyDto.Currency, currencyDto.Rate);
    }
}
