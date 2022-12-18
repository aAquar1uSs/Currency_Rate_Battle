using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface ICurrencyRepository
{
    Task UpdateAsync(Currency[] currencies, CancellationToken cancellationToken);
}
