
namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRateCalculationService
{
    Task StartRateCalculationByRoomIdAsync(Guid roomId);
}
