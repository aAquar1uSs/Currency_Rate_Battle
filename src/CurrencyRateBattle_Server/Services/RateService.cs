using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services;

public class RateService : IRateService
{
    private readonly ILogger<IAccountService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public RateService(ILogger<AccountService> logger,
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
    public async void UpdateRateAsync(Guid id, Rate updatedRate)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        Rate rate;

        await _semaphoreSlim.WaitAsync();
        try
        {
            rate = await db.Rates.FirstOrDefaultAsync(r => r.Id == id);
            if (rate == null)
                throw new CustomException($"{nameof(Rate)} with Id={id} is not found.");
            db.Entry(rate).Property(x => x.IsClosed).CurrentValue = updatedRate.IsClosed;
            db.Entry(rate).Property(x => x.IsWon).CurrentValue = updatedRate.IsWon;
            db.Entry(rate).Property(x => x.Amount).CurrentValue = updatedRate.Amount;
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
            Currency currency;
            await _semaphoreSlim.WaitAsync();
            try
            {
                currency = await db.Currencies.FirstOrDefaultAsync(c => c.CurrencyName == currencyCode);
                currencyId = currency is not null ? currency.Id : throw new CustomException($"Rates can not be retrieved: currency is invalid.");
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
                : isActive == false ? await db.Rates.Where(r => r.IsClosed).ToListAsync() : await db.Rates.ToListAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return result;
    }
    public async Task<List<Rate>> GetRatesByAccountIdAsync(Guid accountId)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        List<Rate> result;
        await _semaphoreSlim.WaitAsync();
        try
        {
            result = await db.Rates.Where(r => r.AccountId == accountId).ToListAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return result;
    }

    public async void DeleteRateAsync(Guid id)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        Rate rateToDelete;
        await _semaphoreSlim.WaitAsync();
        try
        {
            rateToDelete = await db.Rates.FindAsync(id);
            db.Rates.Remove(rateToDelete);
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }
    }
}
