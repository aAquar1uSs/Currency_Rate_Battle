using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Managers;

public interface IJwtManager
{
    Tokens Authenticate(User user);
}
