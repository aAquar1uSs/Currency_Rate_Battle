using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Converters;

public static class UserConverter
{
    public static UserDal ToDal(this User user)
    {
        return new UserDal
        {
            Id = user.Id.Id,
            Email = user.Email.Value,
            Password = user.Password.Value
        };
    }

    public static User ToDomain(this UserDal userDal)
    {
        return User.Create(userDal.Id, userDal.Email, userDal.Password);
    }
}
