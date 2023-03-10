using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface IRateRepository
{
    Task Create(Rate rate, CancellationToken cancellationToken);

    Task UpdateRange(Rate[] updatedRate, CancellationToken cancellationToken);

    Task<Rate[]> Find(bool? isActive, string? currencyName, CancellationToken cancellationToken);
}
