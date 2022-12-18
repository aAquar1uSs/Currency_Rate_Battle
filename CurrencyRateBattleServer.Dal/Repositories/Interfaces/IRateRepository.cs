using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRateRepository
{
    Task CreateAsync(Rate rate, CancellationToken cancellationToken);

    Task<Rate[]> GetRateByRoomIdAsync(Guid roomId);

    Task DeleteRateAsync(Rate rateToDelete);

    Task UpdateRateByRoomIdAsync(Guid id, RateDal updatedRateDal);

    Task<Rate[]> FindAsync(bool? isActive, string? currencyCode, CancellationToken cancellationToken);
}
