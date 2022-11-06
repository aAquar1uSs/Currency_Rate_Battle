using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IRateService
{
    Task<RateDal> CreateRateAsync(Rate rate, Guid accountId, Guid currencyId);

    Task<List<RateDal>> GetRateByRoomIdAsync(Guid roomId);

    Task DeleteRateAsync(Guid id);

    Task UpdateRateByRoomIdAsync(Guid id, RateDal updatedRateDal);

    Task<Rate[]> GetRatesAsync(bool? isActive, string? currencyCode);

    Task<Bet[]> GetRatesByAccountIdAsync(Guid accountId);
}
