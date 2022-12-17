using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IAccountHistoryRepository
{
    /// <summary>
    /// Get a list of AccountHistory models from DataBase by account id;
    /// </summary>
    /// <param name="id"> Account Id;</param>
    /// <returns>
    /// the list <see cref="List{T}"/> of AccountHistory models <see cref="AccountHistory"/>;
    /// </returns>
    Task<AccountHistory[]> GetAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new account history;
    /// </summary>
    /// <param name="accountHistory"><see cref="AccountHistory"/>;</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task CreateAsync(AccountHistory accountHistory, CancellationToken cancellationToken);
}
