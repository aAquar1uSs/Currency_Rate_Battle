using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class CurrencyStateRepository : ICurrencyStateRepository
{
    private readonly ILogger<CurrencyStateRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public CurrencyStateRepository(ILogger<CurrencyStateRepository> logger,
        CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<string> GetCurrencyStateIdByRoomIdAsync(RoomId roomId, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateIdByRoomIdAsync)}, was caused.");

        var currId = await _dbContext.CurrencyStates
            .Where(currState => currState.Room.Id == roomId.Id)
            .Select(currState => currState.CurrencyCode).FirstAsync(cancellationToken);

        return currId;
    }

    public async Task<CurrencyState?> GetCurrencyStateByRoomIdAsync(RoomId roomId, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateByRoomIdAsync)}, was caused.");

        var currencyState = await _dbContext.CurrencyStates
            .FirstOrDefaultAsync(currState => currState.Room.Id == roomId.Id, cancellationToken);

        return currencyState?.ToDomain();
    }

    //ToDo Move to CurrencyRepository
    public async Task<Currency[]> GetAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetAsync)} was caused");

        var currency = await _dbContext.Currencies
            .ToArrayAsync(cancellationToken);

        return currency.Select(x => x.ToDomain()).ToArray();
    }
}

