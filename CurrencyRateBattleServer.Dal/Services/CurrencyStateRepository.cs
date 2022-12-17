using System.Globalization;
using System.Text.Json;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class CurrencyStateRepository : ICurrencyStateRepository
{
    private const string NBU_API = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    private readonly ILogger<CurrencyStateRepository> _logger;

    private readonly CurrencyRateBattleContext _dbContext;

    public CurrencyStateRepository(ILogger<CurrencyStateRepository> logger,
        CurrencyRateBattleContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Guid> GetCurrencyIdByRoomIdAsync(Guid roomId)
    {
        _logger.LogDebug($"{nameof(GetCurrencyIdByRoomIdAsync)}, was caused.");

        var currId = await _dbContext.CurrencyStates
            .Where(currState => currState.Room.Id == roomId)
            .Select(currState => currState.Currency.Id).FirstAsync();

        return currId;
    }

    public async Task PrepareUpdateCurrencyRateAsync()
    {
        _logger.LogDebug($"{nameof(PrepareUpdateCurrencyRateAsync)}, was caused.");

        await GetCurrencyRatesFromNbuApiAsync();

        foreach (var room in _dbContext.Rooms)
        {
            if ((room.Date.Date == DateTime.UtcNow.Date
                 && room.Date.Hour == DateTime.UtcNow.Hour)
                || DateTime.UtcNow.Date > room.Date.Date)
            {
                var currencyState = await GetCurrencyStateByRoomIdAsync(room.Id);

                if (currencyState != null)
                    await UpdateCurrencyRateAsync(currencyState);
            }
        }
    }

    public async Task<CurrencyState?> GetCurrencyStateByRoomIdAsync(Guid roomId)
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateByRoomIdAsync)}, was caused.");

        var currencyState = await _dbContext.CurrencyStates
            .FirstOrDefaultAsync(currState => currState.Room.Id == roomId);

        return currencyState.ToDomain();
    }

    public async Task<CurrencyState[]> GetCurrencyStateAsync()
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateAsync)} was caused");

        if (_rateStorage is null)
            return Array.Empty<CurrencyState>();
        
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

    public async Task GetCurrencyRatesFromNbuApiAsync()
    {
        _logger.LogDebug($"{nameof(GetCurrencyRatesFromNbuApiAsync)} was caused");

        using var client = new HttpClient();
        client.BaseAddress = new Uri(NBU_API);

        var stream = await client.GetStreamAsync(NBU_API);
        _rateStorage = await JsonSerializer.DeserializeAsync<List<CurrencyStateDto>>(stream);

        if (_rateStorage is null)
            throw new ArgumentException(nameof(_rateStorage));
    }

    public async Task UpdateCurrencyRateAsync(CurrencyState currencyState)
    {
        _logger.LogDebug($"{nameof(UpdateCurrencyRateAsync)} was caused");

        var currentDate = DateTime.ParseExact(
            DateTime.UtcNow.ToString("MM.dd.yyyy HH:00:00", CultureInfo.InvariantCulture),
            "MM.dd.yyyy HH:mm:ss", null);

        var currencyName = (await _dbContext.Currencies
            .FirstOrDefaultAsync(curr => curr.Id == currencyState.CurrencyId))?.CurrencyName;

        if (_rateStorage is null)
            return;

        var currencyDto = _rateStorage.FirstOrDefault(curr => curr.Currency == currencyName);

        if (currencyDto is null)
            return;

        currencyState.Date = currentDate;
        currencyState.CurrencyExchangeRate = Math.Round(currencyDto.Rate, 2);

        _ = _dbContext.CurrencyStates.Update(currencyState);

        _ = await _dbContext.SaveChangesAsync();
    }
}

