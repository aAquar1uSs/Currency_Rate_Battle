using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRateService
{
    Task<Rate> CreateRateAsync(Rate rate);

    Task UpdateRateAsync(Guid id, Rate updatedRate);

    Task DeleteRateAsync(Guid id);

    Task<List<Rate>> GetRatesAsync(bool? isActive, string? currencyCode);

    Task<List<BetDto>> GetRatesByAccountIdAsync(Guid accountId);

}
