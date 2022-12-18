using CurrencyRateBattleServer.Data;

namespace CurrencyRateBattleServer.Dal.Entities;

public class ResultUserRatingDal
{
    public UserRatingDal TotalQ { get; set; }

    public UserRatingDal WonQ { get; set; }
}
