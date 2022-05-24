using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IPaymentService
{
    Task ApportionCashByRateAsync(Guid accId, decimal amount);
}
