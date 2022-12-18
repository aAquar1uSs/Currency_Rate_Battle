using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IAccountRepository
{
    /// <summary>
    /// Creates new account and gives his unique token <see cref="Tokens"/>;
    /// </summary>
    /// <param name="userData"><see cref="UserDto"/></param>
    /// <returns>
    ///the task result contains <see cref="Tokens"/>;
    /// </returns>
    /// <exception cref="Helpers.GeneralException"> Throws if such user is already taken;</exception>
    public Task CreateAccountAsync(Account account, CancellationToken cancellationToken);

    /// <summary>
    /// Get account model from database by user id;
    /// </summary>
    /// <param name="userId"><see cref="UserDal"/> id;</param>
    /// <returns>
    ///the task result contains <see cref="Account"/>;
    /// </returns>
    public Task<Account?> GetAccountByUserIdAsync(UserId userId, CancellationToken cancellationToken);
}
