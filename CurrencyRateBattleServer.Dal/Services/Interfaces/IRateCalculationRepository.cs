using CurrencyRateBattleServer.Dal.Entities;

namespace CurrencyRateBattleServer.Dal.Services.Interfaces;

public interface IRateCalculationRepository
{
    /// <summary>
    /// Starts the winning and losing betting algorithm.
    /// </summary>
    /// <param name="roomId"><see cref="RoomDal"/> id</param>
    /// <returns>
    ///A task that represents the asynchronous operation. <see cref="Task"/>;
    /// </returns>
    Task StartRateCalculationByRoomIdAsync(Guid roomId);
}
