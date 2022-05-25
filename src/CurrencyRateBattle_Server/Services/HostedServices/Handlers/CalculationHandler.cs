using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.HostedServices.Handlers;

public class CalculationHandler : AbstractHandler
{
    public override Task<List<Rate>> Handle(List<Rate> rates)
    {
        var commonBank = rates.Sum(rate => rate.Amount);

        var winnerCount = rates.Count(rate => rate.IsWon);

        if (winnerCount == 1)
        {
            var rate = rates.FirstOrDefault(rate => rate.IsWon);
            rate.Payout = rate.Amount + (0.5m * (commonBank - rate.Amount));
        }
        else
        {
            var winnerBank = rates
                .Where(rate => rate.IsWon)
                .Sum(rate => rate.Amount);

            var kef = commonBank / winnerBank;

            foreach (var rate in rates)
            {
                if (rate.IsWon)
                    rate.Payout = rate.Amount * kef;
                else
                    rate.Payout = 0;
            }

        }
        return base.Handle(rates);
    }
}
