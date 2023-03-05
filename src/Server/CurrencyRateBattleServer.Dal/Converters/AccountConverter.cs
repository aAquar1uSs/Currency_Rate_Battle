using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class AccountConverter
{
    public static Account ToDomain(this AccountDal accountDal)
    {
        return Account.Create(accountDal.Id, accountDal.Amount, accountDal.Email);
    }

    public static AccountDal ToDal(this Account account)
    {
        return new AccountDal
        {
            Email = account.UserEmail.Value,
            Id = account.Id.Id,
            Amount = account.Amount.Value
        };
    }
}
