﻿using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class AccountHistoryService : IAccountHistoryService
{
    private readonly ILogger<AccountHistoryService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public AccountHistoryService(ILogger<AccountHistoryService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task<List<AccountHistory>> GetAccountHistoryByAccountId(Guid? id)
    {
        _logger.LogDebug($"{nameof(GetAccountHistoryByAccountId)} was caused.");

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var histories = await dbContext.AccountHistory
            .Where(history => history.Account.Id == id)
            .ToListAsync();

        return histories;
    }

    public async Task CreateHistoryAsync(RoomDal? room, Account account,
        AccountHistoryDto accountHistoryDto)
    {
        _logger.LogDebug($"{nameof(CreateHistoryAsync)} was caused.");

        var history = new AccountHistory
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
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        _logger.LogInformation("New history record added to the database.");
    }

    public async Task CreateHistoryByValuesAsync(Guid? roomId, Guid accountId, DateTime recordDate,
        decimal amount, bool isCredit)
    {
        var history = new AccountHistory
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

        _logger.LogInformation("New history record added to the database.");
    }
}
