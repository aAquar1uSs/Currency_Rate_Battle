using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface ICurrencyStateRepository
{
    Task<string> GetCurrencyIdByRoomIdAsync(RoomId roomId, CancellationToken cancellationToken);

    Task<CurrencyState?> GetCurrencyStateByRoomIdAsync(RoomId roomId, CancellationToken cancellationToken);

    /// <summary>
    /// Pulls out all currency state entries from the database;
    /// </summary>
    /// <returns>
    ///the task result contains <see cref="List{T}"/> of <see cref="CurrencyStateDto"/>;
    /// </returns>
    Task<CurrencyState[]> GetCurrencyStateAsync();

    /// <summary>
    /// Updates currency states in the database;
    /// </summary>
    /// <param name="currencyState"> updated currency state which update in database;</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task UpdateCurrencyRateAsync(Currency currency, CancellationToken cancellationToken);
}
