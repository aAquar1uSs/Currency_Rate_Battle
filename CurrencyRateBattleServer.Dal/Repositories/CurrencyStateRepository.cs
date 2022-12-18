using System.Globalization;
using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NbuClient;

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

    public async Task<string> GetCurrencyIdByRoomIdAsync(RoomId roomId, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyIdByRoomIdAsync)}, was caused.");

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

    public async Task<CurrencyState[]> GetCurrencyStateAsync()
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateAsync)} was caused");

        List<CurrencyState> currencyStates = new();
        foreach (var curr in _dbContext.Currencies)
        {
            var item = _rateStorage.Find(x => x.Currency.CurrencyName == curr.CurrencyName);

            if (item is null)
                continue;

            currencyStates.Add(item);
        }
        
        return await Task.FromResult(currencyStates.ToArray());
    }

    public async Task UpdateCurrencyRateAsync(Currency currency, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(UpdateCurrencyRateAsync)} was caused");

        var currentDate = DateTime.ParseExact(
            DateTime.UtcNow.ToString("MM.dd.yyyy HH:00:00", CultureInfo.InvariantCulture),
            "MM.dd.yyyy HH:mm:ss", null);

        var currencyState = (await _dbContext.CurrencyStates
            .FirstOrDefaultAsync(curr => curr.CurrencyCode == currency.CurrencyCode.Value, cancellationToken: cancellationToken));
        if (currencyState is null)
            return;
        
        currencyState.UpdateDate = currentDate;
        currencyState.CurrencyExchangeRate = Math.Round(currency.Rate.Value, 2);

        _ = _dbContext.CurrencyStates.Update(currencyState);

        _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

