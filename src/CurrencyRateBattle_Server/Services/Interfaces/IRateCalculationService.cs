using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IRateCalculationService
{
    Task<List<Rate>> DefinedWinnerOrLoserAsync(List<Rate> rates);

    bool IsCorrectRateStates(List<Rate> rates);

    List<Rate> CalculatePayout(List<Rate> rates);
}
