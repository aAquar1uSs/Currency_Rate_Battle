using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IAccountService
{
    public Task CreateAccountAsync(Account account);

    /// <summary>
    /// Get account model from database by user id;
    /// </summary>
    /// <param name="userId"><see cref="UserDal"/> id;</param>
    /// <returns>
    ///the task result contains <see cref="Account"/>;
    /// </returns>
    public Task<Account?> FindAsync(Guid? userId);
}
