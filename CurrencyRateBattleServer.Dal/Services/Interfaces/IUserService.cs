using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IUserService
{
    public Task<User?> FindAsync(User userData);
    
    public Task CreateAsync(User userData);
}
