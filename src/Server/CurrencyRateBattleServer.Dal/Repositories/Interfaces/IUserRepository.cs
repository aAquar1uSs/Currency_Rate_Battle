using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User?> Get(Email email, Password password, CancellationToken cancellationToken);

    public Task<User?> Find(Email email, CancellationToken cancellationToken);

    public Task Create(User userData, CancellationToken cancellationToken);

}
