using CurrencyRateBattleServer.Dal.Services.HostedServices;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface ICurrencyStateRepository
{
    Task<Guid> GetCurrencyIdByRoomIdAsync(Guid roomId);

    Task<CurrencyState?> GetCurrencyStateByRoomIdAsync(Guid roomId);

    /// <summary>
    /// Checks the closing date of the room if the room is closed ,
    /// performs the currency update in this room;
    /// </summary>
    /// <remarks>
    ///uses <see cref="CurrencyHostedService"/> uses this method;
    /// </remarks>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task PrepareUpdateCurrencyRateAsync();

    /// <summary>
    /// Pulls out all currency state entries from the database;
    /// </summary>
    /// <returns>
    ///the task result contains <see cref="List{T}"/> of <see cref="CurrencyStateDto"/>;
    /// </returns>
    Task<CurrencyState[]> GetCurrencyStateAsync();

    /// <summary>
    /// Sends a request for currency exchange. The received data is recorded in the List.
    /// </summary>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task GetCurrencyRatesFromNbuApiAsync();

    /// <summary>
    /// Updates currency states in the database;
    /// </summary>
    /// <param name="currencyState"> updated currency state which update in database;</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task UpdateCurrencyRateAsync(CurrencyState currencyState);
}
