
namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IPaymentService
{
    Task ApportionCashByRateAsync(Guid accountId, decimal amount);
}
