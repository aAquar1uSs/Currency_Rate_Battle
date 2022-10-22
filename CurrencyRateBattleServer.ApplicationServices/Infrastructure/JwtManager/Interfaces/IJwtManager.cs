using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;

public interface IJwtManager
{
    Tokens Authenticate(User user);
}
