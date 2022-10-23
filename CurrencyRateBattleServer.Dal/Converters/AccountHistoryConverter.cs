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
        return new AccountHistory
        {
            Account = accountHistoryDal.Account.ToDomain(),
            Amount = accountHistoryDal.Amount,
            Date = accountHistoryDal.Date,
            IsCredit = accountHistoryDal.IsCredit,
            Room = accountHistoryDal.Room?.ToDomain()
        };
    }

    public static AccountHistoryDal[] ToDal(this AccountHistory[] accountHistory)
    {
        return accountHistory.Select(dal => dal.ToDal()).ToArray();
    }

    public static AccountHistoryDal ToDal(this AccountHistory accountHistoryDal)
    {
        return new AccountHistoryDal
        {
            Account = accountHistoryDal.Account.ToDal(),
            Amount = accountHistoryDal.Amount,
            Date = accountHistoryDal.Date,
            IsCredit = accountHistoryDal.IsCredit,
            Room = accountHistoryDal.Room?.ToDal()
        };
    }
}
