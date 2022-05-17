using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IAccountService
{
    public Task<Tokens?> LoginAsync(UserDto userData);

    public Task<Tokens?> RegistrationAsync(UserDto userData);

}
