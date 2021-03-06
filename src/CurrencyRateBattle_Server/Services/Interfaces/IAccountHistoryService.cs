using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IAccountHistoryService
{
    /// <summary>
    /// Get a list of AccountHistory models from DataBase by account id;
    /// </summary>
    /// <param name="id"> Account Id;</param>
    /// <returns>
    /// the list <see cref="List{T}"/> of AccountHistory models <see cref="AccountHistory"/>;
    /// </returns>
    Task<List<AccountHistory>> GetAccountHistoryByAccountId(Guid? id);

    /// <summary>
    /// Creates a new account history;
    /// </summary>
    /// <param name="room"><see cref="Room"/> model;</param>
    /// <param name="account"><see cref="Account"/> model;</param>
    /// <param name="accountHistoryDto"><see cref="AccountHistoryDto"/>;</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task CreateHistoryAsync(Room? room, Account account, AccountHistoryDto accountHistoryDto);

    /// <summary>
    /// Creates a new account history by values;
    /// </summary>
    /// <param name="roomId"><see cref="Room"/> Id;</param>
    /// <param name="accountId"><see cref="Account"/> id;</param>
    /// <param name="recordDate">Recording date;</param>
    /// <param name="amount">The amount of money the user put;</param>
    /// <param name="isCredit">Closed rate indicator;</param>
    /// <returns>
    /// A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task CreateHistoryByValuesAsync(Guid? roomId, Guid accountId, DateTime recordDate, decimal amount, bool isCredit);
}
