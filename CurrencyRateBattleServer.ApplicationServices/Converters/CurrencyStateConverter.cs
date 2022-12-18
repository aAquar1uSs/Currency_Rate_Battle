using System.Globalization;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class CurrencyStateConverter
{
    public static CurrencyStateDto[] ToDto(this CurrencyState[] currencyStates)
    {
        return currencyStates.Select(x => x.ToDto()).ToArray();
    }

    private static CurrencyStateDto ToDto(this CurrencyState currencyState)
    {
        return new CurrencyStateDto
        {
            Currency = currencyState.,
            Date = currencyState.Date.ToString(CultureInfo.InvariantCulture),
            Rate = currencyState.CurrencyExchangeRate.Value
    }
}
