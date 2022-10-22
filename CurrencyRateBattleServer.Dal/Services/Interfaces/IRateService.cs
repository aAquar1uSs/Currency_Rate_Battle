using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRateService
{
    Task<RateDal> CreateRateAsync(RateDto rate, Guid accountId, Guid currencyId);

    Task<List<RateDal>> GetRateByRoomIdAsync(Guid roomId);

    Task DeleteRateAsync(Guid id);

    Task UpdateRateByRoomIdAsync(Guid id, RateDal updatedRateDal);

    Task<List<RateDal>> GetRatesAsync(bool? isActive, string? currencyCode);

    Task<List<BetDto>> GetRatesByAccountIdAsync(Guid accountId);

    Task<List<UserRatingDto>> GetUsersRatingAsync();
}
