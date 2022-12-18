using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IPaymentRepository
{
    /// <summary>
    /// Makes a cash payment to the account;
    /// </summary>
    /// <param name="roomId"><see cref="RoomDal"/> id;</param>
    /// <param name="accountId"><see cref="Account"/> id;</param>
    /// <param name="payout">user payment;</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task ApportionCashByRateAsync(Guid roomId, Guid accountId, decimal? payout);

    /// <summary>
    /// Withdraws money from the user account;
    /// </summary>
    /// <param name="accountId"><see cref="Account"/> id;</param>
    /// <param name="amount">The amount of money the user put;</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="bool"/>;
    /// </returns>
    Task<bool> WritingOffMoneyAsync(Account account, decimal? amount);
}
