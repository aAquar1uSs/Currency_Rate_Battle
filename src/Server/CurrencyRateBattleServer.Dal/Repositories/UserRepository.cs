using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CurrencyRateBattleContext _dbContext;

    public UserRepository(CurrencyRateBattleContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<User?> GetAsync(Email email, Password password, CancellationToken cancellationToken)
    {
        var userDal = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(dal => dal.Email == email.Value && dal.Password == password.Value,
                cancellationToken);

        return userDal?.ToDomain();
    }

    public async Task<User?> FindAsync(Email email, CancellationToken cancellationToken)
    {
        var userDal = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(dal => dal.Email == email.Value, cancellationToken);
        return userDal?.ToDomain();
    }

    public async Task CreateAsync(User userData, CancellationToken cancellationToken)
    {
        var userDal = userData.ToDal();

        _ = await _dbContext.Users.AddAsync(userDal, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
