using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Infrastructure;

namespace CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;

public interface IJwtManager
{
    Tokens Authenticate(User user);
}
