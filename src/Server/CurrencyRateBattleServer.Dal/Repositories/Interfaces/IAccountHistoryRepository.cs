using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IAccountHistoryRepository
{
    /// <summary>
    /// Get a list of AccountHistory models from DataBase by account id;
    /// </summary>
    /// <param name="id"> Account Id;</param>
    /// <returns>
    /// the list <see cref="List{T}"/> of AccountHistory models <see cref="AccountHistory"/>;
    /// </returns>
    Task<AccountHistory[]> Get(AccountId id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new account history;
    /// </summary>
    /// <param name="accountHistory"><see cref="AccountHistory"/>;</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task Create(AccountHistory accountHistory, CancellationToken cancellationToken);
}
