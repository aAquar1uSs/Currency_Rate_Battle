

using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface ICurrencyStateService
{
    Task<Guid> GetCurrencyIdByRoomId(Guid roomId);

    Task PrepareUpdateCurrencyRateAsync();

    Task UpdateEmptyCurrencyStateAsync();

    Task GetCurrencyRatesFromNbuApiAsync();

    Task UpdateCurrencyRateByIdAsync(CurrencyState currencyState);
}
