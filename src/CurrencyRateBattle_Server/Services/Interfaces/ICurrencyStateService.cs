

using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface ICurrencyStateService
{
    Task PrepareUpdateCurrencyRateAsync();

    Task UpdateEmptyCurrencyStateAsync();

    Task GetCurrencyRatesFromNbuApiAsync();

    Task UpdateCurrencyRateByIdAsync(CurrencyState currencyState);
}
