using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.HostedServices.Handlers;

public class CalculationHandler : AbstractHandler
{
    public override Task<List<Rate>> Handle(List<Rate> rates)
    {
        var commonBank = rates.Sum(rate => rate.Amount);

        var winnerCount = rates.Count(rate => rate.IsWon);

        if (winnerCount == 0)
        {
            rates.ForEach(rate => rate.Payout = 0m);
        }
        else if (winnerCount == 1)
        {
            var rate = rates.FirstOrDefault(rate => rate.IsWon);
            if (rate != null)
                rate.Payout = rate.Amount + (0.5m * (commonBank - rate.Amount));
        }
        else
        {
            if (CheckSameRates(rates))
                UnusualCalculation(ref rates, commonBank);
            else
                StandartCalculation(ref rates, commonBank);
        }

        return base.Handle(rates);
    }

    private void StandartCalculation(ref List<Rate> rates, decimal commonBank)
    {
        var winnerBank = rates
                .Where(rate => rate.IsWon)
                .Sum(rate => rate.Amount);

        var kef = commonBank / winnerBank;

        foreach (var rate in rates)
        {
            rate.Payout = rate.IsWon ? rate.Amount * kef : 0;
        }
    }

    private void UnusualCalculation(ref List<Rate> rates, decimal commonBank)
    {
        rates.ForEach(rate =>
        {
            if (!rate.IsWon)
                rate.Payout = 0;
        });

        rates.Sort((x, y) => x.SetDate.CompareTo(y.SetDate));

        var winners = rates.Where(rate => rate.IsWon);
        var winnerCount = winners.Count();
        var loserBank = commonBank - winners.Sum(rate => rate.Amount);
        var step = 2 * loserBank / winnerCount * (winnerCount - 1);

        //rates.ForEach((rate, index) =>
        //{

        //});
    }

    private bool CheckSameRates(List<Rate> rates)
    {
        var winnings = rates.Where(rate => rate.IsWon);
        var amount = winnings.First().Amount;

        return winnings.All(rate => rate.Amount == amount);
    }
}
