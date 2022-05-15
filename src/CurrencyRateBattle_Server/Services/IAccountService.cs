using CurrencyRateBattle_Server.Dto;
using CurrencyRateBattle_Server.Models;

namespace CurrencyRateBattle_Server.Services;

public interface IAccountService
{
    public Task<Tokens?> LoginAsync(UserDto userData);

    public Task<Tokens?> RegistrationAsync(UserDto userData);

}
