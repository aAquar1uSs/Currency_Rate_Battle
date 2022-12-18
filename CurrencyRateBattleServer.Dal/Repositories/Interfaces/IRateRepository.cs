using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRateRepository
{
    Task CreateAsync(Rate rate, CancellationToken cancellationToken);

    Task<Rate[]> GetRateByRoomIdAsync(RoomId[] roomIds, CancellationToken cancellationToken);

    Task DeleteRateAsync(Rate rateToDelete);

    Task UpdateRateByRoomIdAsync(Guid id, RateDal updatedRateDal);

    Task<Rate[]> FindAsync(bool? isActive, string? currencyCode, CancellationToken cancellationToken);
}
