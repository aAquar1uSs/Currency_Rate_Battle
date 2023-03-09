using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface ICurrencyRepository
{
    Task Update(Currency currency, CancellationToken cancellationToken);

    Task<Currency[]> Get(CancellationToken cancellationToken);
}
