using CurrencyRateBattleServer.Dal.Data;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
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
    
    public Task<UserRating[]> GetUsersRatingAsync()
    {
        _logger.LogInformation($"{nameof(GetUsersRatingAsync)} was caused.");

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
                     join curr in _dbContext.Currencies on rate.CurrencyName equals curr.CurrencyCode
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
                         RateCurrencyExchange = rate.RateCurrencyExchange,
                         Payout = rate.Payout,
                         RoomDate = room.EndDate,
                         RoomId = rate.RoomId,
                         CurrencyCode = curr.CurrencyCode,
                     };
        return result;
    }

    private IQueryable<BetDal> GetBetSubQuery(IQueryable<BetDal> data)
    {
        var query = from res in data
                    join currState in _dbContext.CurrencyStates
                on new { res.RoomId, res.CurrencyCode } equals new { currState.RoomId, currState.CurrencyCode }
                into gj
                    from subCurr in gj.DefaultIfEmpty()
                    select new BetDal
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
                        CurrencyCode = res.CurrencyCode,
                        CurrencyExchangeRate = subCurr == null ? 0 : subCurr.CurrencyExchangeRate
                    };
        return query;
    }

    private IQueryable<UserRatingDal> GetUserRatingDataFirstQuery()
    {
        var query1 = from rate in _dbContext.Rates
                     join acc in _dbContext.Accounts on rate.AccountId equals acc.Id
                     join user in _dbContext.Users on acc.UserId equals user.Id
                     where rate.IsClosed
                     select new UserRatingDal
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

        return totalQuery;
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
        return wonQuery;
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

        return query;
    }
    
    public Task<Bet[]> FindAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(FindAsync)} was caused.");

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
                WonCurrencyExchange =
                    data.CurrencyExchangeRate == 0 ? null : Math.Round((decimal)data.CurrencyExchangeRate, 2),
                UserCurrencyExchange = Math.Round(data.RateCurrencyExchange, 2),
                PayoutAmount = data.Payout,
                CurrencyName = data.CurrencyCode,
                IsClosed = data.IsClosed,
                RoomDate = data.RoomDate
            });
        }

        betDtoStorage.Sort((bet1, bet2) => bet1.RoomDate.CompareTo(bet2.RoomDate));
        return Task.FromResult(betDtoStorage.ToArray());
    }
}
