using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class UserConverter
{
    public static User ToDomain(this UserDto userDto)
    {
        return new User
        {
            Email = userDto.Email,
            Password = userDto.Password
        };
    }

    public static User ToDomain(this UserDal userDal)
    {
        return new User { Id = userDal.Id, Email = userDal.Email, Password = userDal.Password };
    }
}
