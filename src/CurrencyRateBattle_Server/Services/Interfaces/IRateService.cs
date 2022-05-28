using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRateService
{
    Task<Rate> CreateRateAsync(RateDto rate, Guid accountId, Guid currencyId);

    Task<List<Rate>> GetRateByRoomIdAsync(Guid roomId);

    Task DeleteRateAsync(Guid id);

    Task UpdateRateByRoomIdAsync(Guid id, Rate updatedRate);

    Task<List<Rate>> GetRatesAsync(bool? isActive, string? currencyCode);

    Task<List<BetDto>> GetRatesByAccountIdAsync(Guid accountId);

    Task<List<UserRatingDto>> GetUsersRatingAsync();
}
