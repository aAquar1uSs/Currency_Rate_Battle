using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface ICurrencyRepository
{
    Task UpdateAsync(Currency currency, CancellationToken cancellationToken);

    Task<string[]> GetAllIds(CancellationToken cancellationToken);

    Task<decimal> GetRateByCurrencyName(string currencyName, CancellationToken cancellationToken);

    Task<Currency[]> GetAsync(CancellationToken cancellationToken);
}
