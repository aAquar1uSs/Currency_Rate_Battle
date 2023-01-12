using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Verifies the presence of a user , if any, gives his unique token <see cref="Tokens"/>;
    /// </summary>
    /// <param name="userData"><see cref="UserDto"/></param>
    /// <returns>
    ///the task result contains <see cref="Tokens"/>
    /// </returns>
    public Task<User?> GetAsync(User userData, CancellationToken cancellationToken);

    public Task<User?> FindAsync(UserId id, CancellationToken cancellationToken);

    public Task CreateAsync(User userData, CancellationToken cancellationToken);

}
