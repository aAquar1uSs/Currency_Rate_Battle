namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IRatingService
{
    Task<List<UserRatingDto>> GetUsersRatingAsync();
}
