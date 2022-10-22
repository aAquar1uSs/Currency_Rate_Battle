using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class AccountConverter
{
    public static Account ToDomain(this AccountDal accountDal)
    {
        return new Account { Amount = accountDal.Amount };
    }

    public static AccountDal ToDal(this Account account)
    {
        return new AccountDal { Amount = account.Amount };
    }
}
