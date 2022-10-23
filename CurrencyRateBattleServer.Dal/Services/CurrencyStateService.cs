﻿using CurrencyRateBattleServer.Services.Interfaces;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using CurrencyRateBattleServer.Dal;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Services;

public class CurrencyStateService : ICurrencyStateService
{
    private const string NBU_API = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

    private readonly ILogger<CurrencyStateService> _logger;

    private List<CurrencyState>? _rateStorage;

    private readonly CurrencyRateBattleContext _dbContext;

    public CurrencyStateService(ILogger<CurrencyStateService> logger,
        CurrencyRateBattleContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _rateStorage = new List<CurrencyStateDto>();
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

    public async Task<List<CurrencyState>> GetCurrencyStateAsync()
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateAsync)} was caused");

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        List<CurrencyState> currencyStates = new();

        if (_rateStorage is null)
            return currencyStates;

        await _semaphoreSlim.WaitAsync();
        try
        {
            foreach (var curr in dbContext.Currencies)
            {
                var item = _rateStorage.Find(x => x.Currency == curr.CurrencyName);

                if (item is null)
                    continue;

                currencyStates.Add(item);
            }
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return await Task.FromResult(currencyStates);
    }

    public async Task GetCurrencyRatesFromNbuApiAsync()
    {
        _logger.LogDebug($"{nameof(GetCurrencyRatesFromNbuApiAsync)} was caused");

        using var client = new HttpClient();
        await _semaphoreSlimHosted.WaitAsync();
        try
        {
            client.BaseAddress = new Uri(NBU_API);

            var stream = await client.GetStreamAsync(NBU_API);
            _rateStorage = await JsonSerializer.DeserializeAsync<List<CurrencyStateDto>>(stream);

            if (_rateStorage is null)
                throw new ArgumentException(nameof(_rateStorage));
        }
        catch (HttpRequestException httpRequestException)
        {
            _logger.LogError("{Stack}", httpRequestException.StackTrace);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("{Msg}", ex.Message);
        }
        finally
        {
            _ = _semaphoreSlimHosted.Release();
        }
    }

    public async Task UpdateCurrencyRateAsync(CurrencyState currencyState)
    {
        _logger.LogDebug($"{nameof(UpdateCurrencyRateAsync)} was caused");

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var currentDate = DateTime.ParseExact(
            DateTime.UtcNow.ToString("MM.dd.yyyy HH:00:00", CultureInfo.InvariantCulture),
            "MM.dd.yyyy HH:mm:ss", null);

        await _semaphoreSlimHosted.WaitAsync();
        try
        {
            var currencyName = (await db.Currencies
                .FirstOrDefaultAsync(curr => curr.Id == currencyState.CurrencyId))?.CurrencyName;

            if (_rateStorage is null)
                return;

            var currencyDto = _rateStorage.FirstOrDefault(curr => curr.Currency == currencyName);

            if (currencyDto is null)
                return;

            currencyState.Date = currentDate;
            currencyState.CurrencyExchangeRate = Math.Round(currencyDto.Rate, 2);

            _ = db.CurrencyStates.Update(currencyState);

            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlimHosted.Release();
        }
    }
}
