﻿using CurrencyRateBattleServer.Dal.Data;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class RateService : IRateService
{
    private readonly ILogger<RateService> _logger;

    private readonly CurrencyRateBattleContext _dbContext;

    public RateService(ILogger<RateService> logger, CurrencyRateBattleContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(dbContext);

        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<RateDal> CreateRateAsync(Rate rate, Guid accountId, Guid currencyId)
    {
        //ToDo move to handler
        var newRate = new RateDal
        {
            RateCurrencyExchange = rate.UserCurrencyExchange,
            Amount = rate.Amount,
            SetDate = DateTime.UtcNow,
            RoomId = rate.RoomId,
            AccountId = accountId,
            CurrencyId = currencyId
        };

        newRate = _dbContext.Rates.Add(newRate).Entity;
        _ = await _dbContext.SaveChangesAsync();

        return newRate ?? throw new GeneralException($"{nameof(RateDal)} can not be created.");
    }

    public async Task<List<RateDal>> GetRateByRoomIdAsync(Guid roomId)
    {
        _logger.LogInformation($"{nameof(GetRateByRoomIdAsync)} was caused.");

        var rates = await _dbContext.Rates
            .Where(dal => dal.Room.Id == roomId)
            .ToListAsync();

        return rates;
    }

    public async Task UpdateRateByRoomIdAsync(Guid id, RateDal updatedRateDal)
    {
        _logger.LogInformation($"{nameof(UpdateRateByRoomIdAsync)} was caused.");

        var rateExists = await _dbContext.Rooms.AnyAsync(r => r.Id == id);
        if (!rateExists)
            throw new GeneralException($"{nameof(RateDal)} with Id={id} is not found.");

        updatedRateDal.SettleDate = DateTime.UtcNow;

        _ = _dbContext.Rates.Update(updatedRateDal);
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task<Rate[]> GetRatesAsync(bool? isActive, string? currencyCode)
    {
        _logger.LogInformation($"{nameof(GetRatesAsync)} was caused.");

        Guid? currencyId = Guid.Empty;
        if (currencyCode is not null)
        {
            var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.CurrencyName == currencyCode);
            currencyId = currency?.Id ?? throw new GeneralException($"Rates can not be retrieved: currency is invalid.");
        }

        List<RateDal> result;
        if (currencyId != Guid.Empty)
            result = await _dbContext.Rates.Where(dal => dal.Currency.Id == currencyId).ToListAsync();

        result = isActive switch
        {
            null => await _dbContext.Rates.ToListAsync(),
            true => await _dbContext.Rates.Where(r => !r.IsClosed).ToListAsync(),
            _ => await _dbContext.Rates.Where(r => r.IsClosed).ToListAsync()
        };

        return result;
    }

    public Task<List<BetDto>> GetRatesByAccountIdAsync(Guid accountId)
    {
        _logger.LogInformation($"{nameof(GetRatesByAccountIdAsync)} was caused.");

        List<BetDto> betDtoStorage = new();

        var firstQuery = GetBetData(accountId);
        var secondQuery = GetBetSubQuery(firstQuery);

        foreach (var data in secondQuery)
        {
            betDtoStorage.Add(new BetDto
            {
                Id = data.RateId,
                SetDate = data.RateSetDate,
                BetAmount = (decimal)data.Amount,
                SettleDate = data.RateSettleDate,
                WonCurrencyExchange =
                    data.CurrencyExchangeRate == 0 ? null : Math.Round((decimal)data.CurrencyExchangeRate, 2),
                UserCurrencyExchange = Math.Round(data.RateCurrencyExchange, 2),
                PayoutAmount = data.Payout,
                СurrencyName = data.CurrencyName,
                IsClosed = data.IsClosed,
                RoomDate = data.RoomDate
            });
        }

        betDtoStorage.Sort((bet1, bet2) => bet1.RoomDate.CompareTo(bet2.RoomDate));
        return Task.FromResult(betDtoStorage);
    }

    public async Task DeleteRateAsync(Guid id)
    {
        _logger.LogInformation($"{nameof(DeleteRateAsync)} was caused.");

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await _semaphoreSlim.WaitAsync();
        try
        {
            var rateToDelete = await db.Rates.FindAsync(id);
            if (rateToDelete != null)
            {
                _ = db.Rates.Remove(rateToDelete);
                _ = await db.SaveChangesAsync();
            }
            else
            {
                _logger.LogDebug("No rate found to be deleted.");
            }
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }
    }

    public Task<List<UserRatingDto>> GetUsersRatingAsync()
    {
        _logger.LogInformation($"{nameof(GetUsersRatingAsync)} was caused.");

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        List<UserRatingDto> userRatings = new();

        var query1 = GetUserRatingDataFirstQuery(db);
        var totalQuery = GetUserRatingDataTotalQuery(query1);
        var wonQuery = GetUserRatingDataWonQuery(query1);
        var query = GetUserRatingDataTotalWonQuery(totalQuery, wonQuery);

        foreach (var data in query)
        {
            userRatings.Add(new UserRatingDto
            {
                Email = data.TotalQ.UserEmail,
                BetsNo = data.TotalQ.TotalBetCount,
                WonBetsNo = data.WonQ.WonBetCount,
                LastBetDate = data.TotalQ.LastBetDate,
                ProfitPercentage = ((decimal)data.WonQ.WonBetAmount) / (decimal)data.TotalQ.TotalBetAmount,
                WonBetsPercentage = (decimal)data.WonQ.WonBetCount / data.TotalQ.TotalBetCount
            });
        }

        return Task.FromResult(userRatings);
    }

    private static IQueryable<BetData> GetBetData(CurrencyRateBattleContext db, Guid accountId)
    {
        var result = from rate in db.Rates
                     join curr in db.Currencies on rate.CurrencyId equals curr.Id
                     join room in db.Rooms on rate.RoomId equals room.Id
                     where rate.AccountId == accountId
                     select new BetData
                     {
                         RateId = rate.Id,
                         Amount = rate.Amount,
                         RateSettleDate = rate.SettleDate,
                         RateSetDate = rate.SetDate,
                         IsWon = rate.IsWon,
                         IsClosed = rate.IsClosed,
                         AccountId = rate.AccountId,
                         RateCurrencyExchange = rate.RateCurrencyExchange,
                         Payout = rate.Payout,
                         RoomDate = room.Date,
                         RoomId = rate.RoomId,
                         CurrencyName = curr.CurrencyName,
                         CurrencyId = rate.CurrencyId
                     };
        return result;
    }

    private static IQueryable<BetData> GetBetSubQuery(CurrencyRateBattleContext db, IQueryable<BetData> data)
    {
        var query = from res in data
                    join currState in db.CurrencyStates
                on new { res.RoomId, res.CurrencyId } equals new { currState.RoomId, currState.CurrencyId }
                into gj
                    from subCurr in gj.DefaultIfEmpty()
                    select new BetData
                    {
                        RateId = res.RateId,
                        Amount = res.Amount,
                        RateSettleDate = res.RateSettleDate,
                        RateSetDate = res.RateSetDate,
                        IsWon = res.IsWon,
                        IsClosed = res.IsClosed,
                        AccountId = res.AccountId,
                        RateCurrencyExchange = res.RateCurrencyExchange,
                        Payout = res.Payout,
                        RoomDate = res.RoomDate,
                        RoomId = res.RoomId,
                        CurrencyName = res.CurrencyName,
                        CurrencyId = res.CurrencyId,
                        CurrencyExchangeRate = subCurr == null ? 0 : subCurr.CurrencyExchangeRate
                    };
        return query;
    }

    private static IQueryable<UserRatingData> GetUserRatingDataFirstQuery(CurrencyRateBattleContext db)
    {
        var query1 = from rate in db.Rates
                     join acc in db.Accounts on rate.AccountId equals acc.Id
                     join user in db.Users on acc.UserId equals user.Id
                     where rate.IsClosed
                     select new UserRatingData
                     {
                         UserEmail = user.Email,
                         AccountId = rate.AccountId,
                         BetAmount = rate.Amount,
                         IsWon = rate.IsWon,
                         RateSetDate = rate.SetDate,
                         RatePayout = rate.Payout
                     };

        return query1;
    }

    private static IQueryable<UserRatingData> GetUserRatingDataTotalQuery(IQueryable<UserRatingData> data)
    {
        var totalQuery = from res in data
                         group res by new { res.AccountId, res.UserEmail }
                         into grp
                         select new UserRatingData
                         {
                             AccountId = grp.Key.AccountId,
                             UserEmail = grp.Key.UserEmail,
                             LastBetDate = grp.Max(s => s.RateSetDate),
                             TotalBetAmount = grp.Sum(s => s.BetAmount),
                             TotalPayout = grp.Sum(s => s.RatePayout),
                             TotalBetCount = grp.Count()
                         };

        return totalQuery;
    }

    private static IQueryable<UserRatingData> GetUserRatingDataWonQuery(IQueryable<UserRatingData> data)
    {
        var wonQuery = from res in data
                       where res.IsWon
                       group res by new { res.AccountId, res.UserEmail }
                       into grp
                       select new UserRatingData()
                       {
                           AccountId = grp.Key.AccountId,
                           UserEmail = grp.Key.UserEmail,
                           WonBetAmount = grp.Sum(s => s.BetAmount),
                           WonPayout = grp.Sum(s => s.RatePayout),
                           WonBetCount = grp.Count()
                       };
        return wonQuery;
    }

    private static IQueryable<ResultUserRatingData> GetUserRatingDataTotalWonQuery(IQueryable<UserRatingData> data,
        IQueryable<UserRatingData> wonQuery)
    {
        var query = from totalQ in data
                    from wonQ in wonQuery.DefaultIfEmpty()
                    where totalQ.AccountId == wonQ.AccountId
                    select new ResultUserRatingData
                    {
                        TotalQ = totalQ,
                        WonQ = wonQ
                    };

        return query;
    }
}
