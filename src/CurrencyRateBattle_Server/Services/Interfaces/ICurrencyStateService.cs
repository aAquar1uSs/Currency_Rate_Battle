using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface ICurrencyStateService
{
    /// <summary>
    /// Looking for id currency by room id;
    /// </summary>
    /// <param name="roomId"><see cref="Room"/> id;</param>
    /// <returns>
    ///the task result contains <see cref="Guid"/> - <see cref="Currency"/> id;
    /// </returns>
    Task<Guid> GetCurrencyIdByRoomIdAsync(Guid roomId);

    /// <summary>
    /// Search <see cref="CurrencyState"/> model by <see cref="Room"/> id;
    /// </summary>
    /// <param name="roomId"><see cref="Room"/> id;</param>
    /// <returns>
    /// the task result contains <see cref="CurrencyState"/>;
    /// </returns>
    Task<CurrencyState?> GetCurrencyStateByRoomIdAsync(Guid roomId);

    /// <summary>
    /// Checks the closing date of the room if the room is closed ,
    /// performs the currency update in this room;
    /// </summary>
    /// <remarks>
    /// This method uses <see cref="HostedServices.CurrencyHostedService"/>
    /// </remarks>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task PrepareUpdateCurrencyRateAsync();

    /// <summary>
    /// Pulls out all currency state entries from the data base;
    /// </summary>
    /// <returns>
    ///the task result contains <see cref="List{T}"/> of <see cref="CurrencyStateDto"/>;
    /// </returns>
    Task<List<CurrencyStateDto>> GetCurrencyStateAsync();

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
