using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class UserConverter
{
    public static UserDal ToDal(this User user)
    {
        return new UserDal { Email = user.Email, Password = user.Password, Account = user.Account.ToDal() };
    }

    public static User ToDomain(this UserDal userDal)
    {
        return new User { Account = userDal.Account.ToDomain(), Email = userDal.Email, Password = userDal.Password };
    }
}
