using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class AccountHistoryConverter
{
    public static AccountHistory[] ToDomain(this AccountHistoryDal[] accountHistoryDals)
    {
        return accountHistoryDals.Select(dal => dal.ToDomain()).ToArray();
    }

    public static AccountHistory ToDomain(this AccountHistoryDal accountHistoryDal)
    {
        return AccountHistory.Create(accountHistoryDal.Id, accountHistoryDal.AccountId, accountHistoryDal.Date,
            accountHistoryDal.Amount, accountHistoryDal.IsCredit, accountHistoryDal.RoomId);
    }

    public static AccountHistoryDal[] ToDal(this AccountHistory[] accountHistory)
    {
        return accountHistory.Select(dal => dal.ToDal()).ToArray();
    }

    public static AccountHistoryDal ToDal(this AccountHistory accountHistory)
    {
        return new AccountHistoryDal
        {
            Id = accountHistory.Id.Id,
            AccountId = accountHistory.AccountId.Id,
            Amount = accountHistory.Amount.Value,
            Date = accountHistory.Date,
            IsCredit = accountHistory.IsCredit,
            RoomId = accountHistory.RoomId?.Id
        };
    }
}
