using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class UserRatingConverter
{
    public static UserRatingDto ToDto(this UserRating rating)
    {
        return new()
        {
            BetsNo = rating.BetsNo,
            Email = rating.Email,
            LastBetDate = rating.LastBetDate,
            ProfitPercentage = rating.ProfitPercentage,
            WonBetsNo = rating.WonBetsNo,
            WonBetsPercentage = rating.WonBetsPercentage
        };
    }
}
