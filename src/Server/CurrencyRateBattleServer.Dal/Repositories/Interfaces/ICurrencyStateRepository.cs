using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface ICurrencyStateRepository
{
    Task<string> GetCurrencyStateIdByRoomIdAsync(RoomId roomId, CancellationToken cancellationToken);

    Task<CurrencyState?> GetCurrencyStateByRoomIdAsync(RoomId roomId, CancellationToken cancellationToken);

    /// <summary>
    /// Pulls out all currency state entries from the database;
    /// </summary>
    /// <returns>
    ///the task result contains <see cref="List{T}"/> of <see cref="CurrencyStateDto"/>;
    /// </returns>
    Task<Currency[]> GetAsync(CancellationToken cancellationToken);
}
