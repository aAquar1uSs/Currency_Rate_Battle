using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface ICurrencyRepository
{
    Task UpdateAsync(Currency[] currencies, CancellationToken cancellationToken);

    Task<string[]> GetAllIds(CancellationToken cancellationToken);

    Task<decimal> GetCurrencyByCurrencyName(string currencyName, CancellationToken cancellationToken);
}
