using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public UserRepository(ILogger<UserRepository> logger, CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<User?> GetAsync(User userData, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetAsync)} was caused.");

        var userDal = await _dbContext.Users
            .FirstOrDefaultAsync(dal => dal.Email == userData.Email.Value && dal.Password == userData.Password.Value,
                cancellationToken);

        return userDal?.ToDomain();
    }

    public async Task<User?> FindAsync(UserId id, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(FindAsync)} was caused.");

        var userDal = await _dbContext.Users
            .FirstOrDefaultAsync(dal => dal.Id == id.Id, cancellationToken);
        return userDal?.ToDomain();
    }

    public async Task CreateAsync(User userData, CancellationToken cancellationToken)
    {
        var userDal = userData.ToDal();

        _ = await _dbContext.Users.AddAsync(userDal, cancellationToken);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("New user added to the database");
    }
}
