using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services;

public class AccountHistoryService : IAccountHistoryService
{
    private readonly ILogger<AccountHistoryService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public AccountHistoryService(ILogger<AccountHistoryService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task<List<AccountHistory>> GetAccountHistoryByAccountId(Guid? id)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        List<AccountHistory> histories;
        await _semaphoreSlim.WaitAsync();
        try
        {
            histories = await dbContext.AccountHistory
                .Where(history => history.AccountId == id)
                .ToListAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return histories;
    }

    public async Task CreateHistoryAsync(Room? room, Account account,
        AccountHistoryDto accountHistoryDto)
    {
        var history = new AccountHistory()
        {
            Date = accountHistoryDto.Date,
            Amount = accountHistoryDto.Amount,
            IsCredit = accountHistoryDto.IsCredit,
            AccountId = account.Id,
            Account = account
        };
        if (room is not null)
        {
            history.Room = room;
            history.RoomId = room.Id;
        }

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await _semaphoreSlim.WaitAsync();
        try
        {
            _ = await dbContext.AccountHistory.AddAsync(history);
            _ = await dbContext.SaveChangesAsync();
            _logger.LogInformation("New history record is added to the database.");
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }
    }

    public async Task CreateHistoryByValuesAsync(Guid? roomId, Guid accountId, DateTime recordDate, decimal amount, bool isCredit)
    {
        var history = new AccountHistory()
        {
            Date = recordDate,
            Amount = amount,
            IsCredit = isCredit,
            RoomId = roomId,
            AccountId = accountId
        };

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await _semaphoreSlim.WaitAsync();
        try
        {
            _ = await dbContext.AccountHistory.AddAsync(history);
            _ = await dbContext.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }
    }
}
