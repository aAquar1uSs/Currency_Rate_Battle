

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface ICurrencyStateService
{
    Task GetCurrencyRateByNameFromNbuApiAsync();

    Task UpdateCurrencyRateByIdAsync(Guid currId);
}
