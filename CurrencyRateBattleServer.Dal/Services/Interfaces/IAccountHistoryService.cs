using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IAccountHistoryService
{
    /// <summary>
    /// Get a list of AccountHistory models from DataBase by account id;
    /// </summary>
    /// <param name="id"> Account Id;</param>
    /// <returns>
    /// the list <see cref="List{T}"/> of AccountHistory models <see cref="AccountHistory"/>;
    /// </returns>
    Task<AccountHistory[]> GetAccountHistoryByAccountId(Guid? id);

    /// <summary>
    /// Creates a new account history;
    /// </summary>
    /// <param name="accountHistory"><see cref="AccountHistory"/>;</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task CreateHistoryAsync(AccountHistory accountHistory);
}
