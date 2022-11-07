using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class UserService : IUserService
{
    private readonly ILogger<IUserService> _logger;

    private readonly CurrencyRateBattleContext _dbContext;

    public UserService(ILogger<UserService> logger, CurrencyRateBattleContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(dbContext);

        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<User?> FindAsync(User userData)
    {
        _logger.LogDebug($"{nameof(FindAsync)} was caused.");

        var userDal = await _dbContext.Users
            .FirstOrDefaultAsync(dal => dal.Email == userData.Email && dal.Password == userData.Password);

        return userDal?.ToDomain();
    }
    
    public async Task CreateAsync(User userData)
    {
        var userDal = userData.ToDal();

        _ = await _dbContext.Users.AddAsync(userDal);
        _ = await _dbContext.SaveChangesAsync();

        _logger.LogInformation("New user added to the database");
    }
}
