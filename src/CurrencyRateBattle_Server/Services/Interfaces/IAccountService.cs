using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IAccountService
{
    public Task<Tokens?> GetUserAsync(UserDto userData);

    public Task<Tokens?> СreateUserAsync(UserDto userData);

    public Task<AccountInfoDto?> GetAccountInfoAsync(Guid id);

    public Task<Account?> GetAccountByUserIdAsync(Guid? userId);

    public Guid? GetGuidFromRequest(HttpContext context);

}
