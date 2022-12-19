using System.Globalization;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class CurrencyConverter
{
    public static CurrencyDto[] ToDto(this CurrencyState[] currencyStates)
    {
        return currencyStates.Select(x => x.ToDto()).ToArray();
    }

    private static CurrencyDto ToDto(this CurrencyState currencyState)
    {
        return new CurrencyDto
        {
            Currency = currencyState.CurrencyCode.Value,
            Date = currencyState.Date.ToString(CultureInfo.InvariantCulture),
            Rate = currencyState.CurrencyExchangeRate.Value
        };

    }

    public static Currency ToDomain(this NbuClient.Dto.CurrencyDto currencyDto)
    {
        return Currency.Create(currencyDto.Currency, currencyDto.Rate);
    }
}
