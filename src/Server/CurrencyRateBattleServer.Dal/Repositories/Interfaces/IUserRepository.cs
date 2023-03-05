using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetAsync(Email email, Password password, CancellationToken cancellationToken);

    public Task<User?> FindAsync(Email email, CancellationToken cancellationToken);

    public Task CreateAsync(User userData, CancellationToken cancellationToken);

}
