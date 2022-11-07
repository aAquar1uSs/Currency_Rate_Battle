using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IRateService
{
    Task CreateAsync(Rate rate, Guid accountId, Guid currencyId);

    Task<Rate[]> GetRateByRoomIdAsync(Guid roomId);

    Task DeleteRateAsync(Rate rateToDelete);

    Task UpdateRateByRoomIdAsync(Guid id, RateDal updatedRateDal);

    Task<Rate[]> FindAsync(bool? isActive, string? currencyCode);

    Task<BetInfo[]> FindAsync(Guid accountId);
}
