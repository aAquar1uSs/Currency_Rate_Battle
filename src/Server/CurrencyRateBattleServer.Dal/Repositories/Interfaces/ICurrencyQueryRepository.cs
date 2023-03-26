namespace CurrencyRateBattleServer.Dal.Repositories.Interfaces;

public interface ICurrencyQueryRepository
{
    Task<string[]> GetAllIds(CancellationToken cancellationToken);
    
    Task<decimal> GetRateByCurrencyName(string currencyName, CancellationToken cancellationToken);
}
