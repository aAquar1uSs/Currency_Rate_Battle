using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services;

public interface IAccountService
{
    public Task<Tokens?> LoginAsync(UserDto userData);

    public Task<Tokens?> RegistrationAsync(UserDto userData);

}
