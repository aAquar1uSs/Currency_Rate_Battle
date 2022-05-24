﻿using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services;

public class RateService : IRateService
{
    private readonly ILogger<IRateService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public RateService(ILogger<IRateService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task<Rate> CreateRateAsync(Rate rate)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();
        Rate newRate;
        await _semaphoreSlim.WaitAsync();
        try
        {
            newRate = db.Rates.Add(rate).Entity;
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return newRate ?? throw new CustomException($"{nameof(Rate)} can not be created.");
    }

    public async Task<List<Rate>> GetRateByRoomIdAsync(Guid roomId)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var rates = await db.Rates
            .Where(rate => rate.RoomId == roomId)
            .ToListAsync();

        return rates;
    }

    public async void UpdateRateAsync(Guid id, Rate updatedRate)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await _semaphoreSlim.WaitAsync();
        try
        {
            var rateExists = await db.Rooms.AnyAsync(r => r.Id == id);
            if (!rateExists)
                throw new CustomException($"{nameof(Rate)} with Id={id} is not found.");

            _ = db.Rates.Update(updatedRate);
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }
    }

    //TODO: JOIN select needs to be optimized
    public async Task<List<Rate>> GetRatesAsync(bool? isActive, string? currencyCode)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        Guid? currencyId = Guid.Empty;
        if (currencyCode is not null)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var currency = await db.Currencies.FirstOrDefaultAsync(c => c.CurrencyName == currencyCode);
                currencyId = currency is not null
                    ? currency.Id
                    : throw new CustomException($"Rates can not be retrieved: currency is invalid.");
            }
            finally
            {
                _ = _semaphoreSlim.Release();
            }
        }

        List<Rate> result;
        await _semaphoreSlim.WaitAsync();
        try
        {
            if (currencyId != Guid.Empty)
                result = await db.Rates.Where(r => r.CurrencyId == currencyId).ToListAsync();
            result = isActive == true
                ? await db.Rates.Where(r => !r.IsClosed).ToListAsync()
                : isActive == false
                    ? await db.Rates.Where(r => r.IsClosed).ToListAsync()
                    : await db.Rates.ToListAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return result;
    }

    public async Task<List<BetDto>> GetRatesByAccountIdAsync(Guid accountId)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        List<BetDto> betDtoStorage = new();
        await _semaphoreSlim.WaitAsync();
        try
        {
            var result = from rate in db.Rates
                join curr in db.Currencies on rate.CurrencyId equals curr.Id
                join room in db.Rooms on rate.RoomId equals room.Id
                where rate.AccountId == accountId
                select new
                {
                    rate.Id,
                    rate.Amount,
                    rate.SettleDate,
                    rate.SetDate,
                    rate.IsWon,
                    rate.IsClosed,
                    rate.AccountId,
                    rate.RateCurrencyExchange,
                    rate.Payout,
                    room.Date,
                    rate.RoomId,
                    curr.CurrencyName,
                    rate.CurrencyId
                };

            var query = from res in result
                join currState in db.CurrencyStates
                    on new {res.RoomId, res.CurrencyId} equals new {currState.RoomId, currState.CurrencyId}
                    into gj
                from subCurr in gj.DefaultIfEmpty()
                select new
                {
                    res.Id,
                    res.Amount,
                    res.SettleDate,
                    res.SetDate,
                    res.IsWon,
                    res.IsClosed,
                    res.AccountId,
                    res.RateCurrencyExchange,
                    res.Payout,
                    res.Date,
                    res.RoomId,
                    res.CurrencyName,
                    res.CurrencyId,
                    CurrencyExchangeRate = subCurr == null ? 0 : subCurr.CurrencyExchangeRate
                };


            foreach (var data in query)
            {
                betDtoStorage.Add(new BetDto
                {
                    Id = data.Id,
                    SetDate = data.SetDate,
                    BetAmount = data.Amount,
                    SettleDate = data.SettleDate,
                    WonCurrencyExchange =
                        data.CurrencyExchangeRate == 0 ? null : Math.Round(data.CurrencyExchangeRate, 2),
                    UserCurrencyExchange = Math.Round(data.RateCurrencyExchange, 2),
                    PayoutAmount = data.Payout,
                    СurrencyName = data.CurrencyName,
                    IsClosed = data.IsClosed,
                    RoomDate = data.Date
                });
            }
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return betDtoStorage;
    }

    public async void DeleteRateAsync(Guid id)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await _semaphoreSlim.WaitAsync();
        try
        {
            var rateToDelete = await db.Rates.FindAsync(id);
            db.Rates.Remove(rateToDelete);
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }
    }

    public async Task<List<Rate>> DefinedWinnerOrLoserAsync(List<Rate> rates)
    {
        var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var currState = await db.CurrencyStates
            .FirstOrDefaultAsync(currState => currState.CurrencyId == rates.First().CurrencyId);

        foreach (var rate in rates)
        {
            rate.IsWon = rate.RateCurrencyExchange == currState.CurrencyExchangeRate;
            rate.IsClosed = true;
        }

        return rates;
    }

    public bool IsCorrectRateStates(List<Rate> rates)
    {
        if (rates.Count <= 1)
            return false;
        var winCount = 0;
        var loseCount = 0;

        foreach (var rate in rates)
        {
            if (rate.IsWon)
                winCount++;
            else
                loseCount++;
        }

        return true;
    }

    public List<Rate> CalculatePayout(List<Rate> rates)
    {
        var commonBank = rates.Sum(rate => rate.Amount);

        var winnerBank = rates
            .Where(rate => rate.IsWon)
            .Sum(rate => rate.Amount);

        var koef = commonBank / winnerBank;

        foreach (var rate in rates)
        {
            if (rate.IsWon)
                rate.Payout = rate.Amount * koef;
            else
                rate.Payout = 0;
        }

        return rates;
    }
}
