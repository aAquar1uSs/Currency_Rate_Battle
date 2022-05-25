

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface ICurrencyStateService
{
    Task PrepareUpdateCurrencyRateAsync();

    Task GetCurrencyRatesFromNbuApiAsync();

    Task UpdateCurrencyRateByIdAsync(Guid currId);
}
