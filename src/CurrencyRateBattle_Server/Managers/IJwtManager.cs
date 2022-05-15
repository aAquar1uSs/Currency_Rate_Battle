using CurrencyRateBattle_Server.Models;

namespace CurrencyRateBattle_Server.Managers;

public interface IJwtManager
{
    Tokens Authenticate(User user);
}
