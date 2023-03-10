using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class UserRatingQueryRepository : IUserRatingQueryRepository
{
    private readonly ILogger<UserRatingQueryRepository> _logger;
    private readonly CurrencyRateBattleContext _dbContext;

    public UserRatingQueryRepository(CurrencyRateBattleContext dbContext, ILogger<UserRatingQueryRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<UserRating[]> GetUsersRating()
    {
        _logger.LogInformation($"{nameof(GetUsersRating)} was caused.");

        List<UserRating> userRatings = new();

        var query1 = GetUserRatingDataFirstQuery();
        var totalQuery = GetUserRatingDataTotalQuery(query1);
        var wonQuery = GetUserRatingDataWonQuery(query1);
        var query = GetUserRatingDataTotalWonQuery(totalQuery, wonQuery);

        foreach (var data in query)
        {
            userRatings.Add(new UserRating
            {
                Email = data.TotalQ.UserEmail,
                BetsNo = data.TotalQ.TotalBetCount,
                WonBetsNo = data.WonQ.WonBetCount,
                LastBetDate = data.TotalQ.LastBetDate,
                ProfitPercentage = ((decimal)data.WonQ.WonBetAmount) / (decimal)data.TotalQ.TotalBetAmount,
                WonBetsPercentage = (decimal)data.WonQ.WonBetCount / data.TotalQ.TotalBetCount
            });
        }

        return Task.FromResult(userRatings.ToArray());
    }

    private IQueryable<BetDal> GetBetData(Guid accountId)
    {
        var result = from rate in _dbContext.Rates
                     join curr in _dbContext.Currencies on rate.CurrencyName equals curr.CurrencyName
                     join room in _dbContext.Rooms on rate.RoomId equals room.Id
                     where rate.AccountId == accountId
                     select new BetDal
                     {
                         RateId = rate.Id,
                         Amount = rate.Amount,
                         RateSettleDate = rate.SettleDate,
                         RateSetDate = rate.SetDate,
                         IsWon = rate.IsWon,
                         IsClosed = rate.IsClosed,
                         AccountId = rate.AccountId,
                         UserRateCurrencyExchange = rate.RateCurrencyExchange,
                         Payout = rate.Payout,
                         RoomDate = room.EndDate,
                         RoomId = rate.RoomId,
                         CurrencyName = curr.CurrencyName,
                     };
        return result;
    }

    private IQueryable<BetDal> GetBetSubQuery(IQueryable<BetDal> data)
    {
        var query = from res in data
                    join rates in _dbContext.Rates
                on new { res.RoomId, res.CurrencyName, res.AccountId } equals new { rates.RoomId, rates.CurrencyName, rates.AccountId }
                into gj from subCurr in gj.DefaultIfEmpty()
                    select new BetDal
                    {
                        RateId = res.RateId,
                        Amount = res.Amount,
                        RateSettleDate = res.RateSettleDate,
                        RateSetDate = res.RateSetDate,
                        IsWon = res.IsWon,
                        IsClosed = res.IsClosed,
                        AccountId = res.AccountId,
                        UserRateCurrencyExchange = res.UserRateCurrencyExchange,
                        Payout = res.Payout,
                        RoomDate = res.RoomDate,
                        RoomId = res.RoomId,
                        CurrencyName = res.CurrencyName,
                        RealCurrencyExchangeRate = subCurr.RateCurrencyExchange
                    };
        return query;
    }

    private IQueryable<UserRatingDal> GetUserRatingDataFirstQuery()
    {
        var query1 = from rate in _dbContext.Rates
                     join acc in _dbContext.Accounts on rate.AccountId equals acc.Id
                     join user in _dbContext.Users on acc.Email equals user.Email
                     where rate.IsClosed == true
                     select new UserRatingDal
                     {
                         UserEmail = user.Email,
                         AccountId = rate.AccountId,
                         BetAmount = rate.Amount,
                         IsWon = rate.IsWon,
                         RateSetDate = rate.SetDate,
                         RatePayout = rate.Payout
                     };

        return query1.AsNoTracking();
    }

    private IQueryable<UserRatingDal> GetUserRatingDataTotalQuery(IQueryable<UserRatingDal> data)
    {
        var totalQuery = from res in data
                         group res by new { res.AccountId, res.UserEmail }
                         into grp
                         select new UserRatingDal
                         {
                             AccountId = grp.Key.AccountId,
                             UserEmail = grp.Key.UserEmail,
                             LastBetDate = grp.Max(s => s.RateSetDate),
                             TotalBetAmount = grp.Sum(s => s.BetAmount),
                             TotalPayout = grp.Sum(s => s.RatePayout),
                             TotalBetCount = grp.Count()
                         };

        return totalQuery.AsNoTracking();
    }

    private IQueryable<UserRatingDal> GetUserRatingDataWonQuery(IQueryable<UserRatingDal> data)
    {
        var wonQuery = from res in data
                       where res.IsWon
                       group res by new { res.AccountId, res.UserEmail }
                       into grp
                       select new UserRatingDal()
                       {
                           AccountId = grp.Key.AccountId,
                           UserEmail = grp.Key.UserEmail,
                           WonBetAmount = grp.Sum(s => s.BetAmount),
                           WonPayout = grp.Sum(s => s.RatePayout),
                           WonBetCount = grp.Count()
                       };
        return wonQuery.AsNoTracking();
    }

    private IQueryable<ResultUserRatingDal> GetUserRatingDataTotalWonQuery(IQueryable<UserRatingDal> data,
        IQueryable<UserRatingDal> wonQuery)
    {
        var query = from totalQ in data
                    from wonQ in wonQuery.DefaultIfEmpty()
                    where totalQ.AccountId == wonQ.AccountId
                    select new ResultUserRatingDal
                    {
                        TotalQ = totalQ,
                        WonQ = wonQ
                    };

        return query.AsNoTracking();
    }

    public Task<Bet[]> Find(AccountId accountId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(Find)} was caused.");

        List<Bet> betDtoStorage = new();

        var firstQuery = GetBetData(accountId.Id);
        var secondQuery = GetBetSubQuery(firstQuery);

        foreach (var data in secondQuery)
        {
            betDtoStorage.Add(new Bet
            {
                Id = data.RateId,
                SetDate = data.RateSetDate,
                BetAmount = (decimal)data.Amount,
                SettleDate = data.RateSettleDate,
                WonCurrencyExchange = Math.Round((decimal)data.RealCurrencyExchangeRate, 2),
                UserCurrencyExchange = Math.Round(data.UserRateCurrencyExchange, 2),
                PayoutAmount = data.Payout,
                CurrencyName = data.CurrencyName,
                IsClosed = data.IsClosed,
                RoomDate = data.RoomDate
            });
        }

        betDtoStorage.Sort((bet1, bet2) => bet1.RoomDate.CompareTo(bet2.RoomDate));
        return Task.FromResult(betDtoStorage.ToArray());
    }
}
