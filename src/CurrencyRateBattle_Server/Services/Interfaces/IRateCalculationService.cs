
namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRateCalculationService
{
    /// <summary>
    /// Starts the winning and losing betting algorithm.
    /// </summary>
    /// <param name="roomId"><see cref="Models.Room"/> id</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task StartRateCalculationByRoomIdAsync(Guid roomId);
}
