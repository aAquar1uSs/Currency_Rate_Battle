using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRateRepository
{
    Task CreateAsync(Rate rate, CancellationToken cancellationToken);

    Task<Rate[]> GetActiveRateByRoomIdsAsync(RoomId[] roomIds, CancellationToken cancellationToken);

    Task DeleteRateAsync(Rate rateToDelete);

    Task UpdateRangeAsync(Rate[] updatedRate, CancellationToken cancellationToken);

    Task<Rate[]> FindAsync(bool? isActive, string? currencyName, CancellationToken cancellationToken);
}
