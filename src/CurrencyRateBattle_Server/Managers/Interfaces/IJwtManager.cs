using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Managers.Interfaces;

public interface IJwtManager
{
    Tokens Authenticate(User user);
}
