
namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IPaymentService
{
    Task ApportionCashByRateAsync(Guid roomId, Guid accountId, decimal? payout);

    Task<bool> WritingOffMoneyAsync(Guid accountId, decimal? amount);
}
