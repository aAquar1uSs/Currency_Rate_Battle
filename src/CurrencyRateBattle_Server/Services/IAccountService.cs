using CurrencyRateBattle_Server.Models;

namespace CurrencyRateBattle_Server.Services;

public interface IAccountService
{
    public Task<Tokens?> LoginAsync(User userData);
}
