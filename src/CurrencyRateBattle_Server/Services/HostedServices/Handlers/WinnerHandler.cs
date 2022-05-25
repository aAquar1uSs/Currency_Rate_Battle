using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services.HostedServices.Handlers;

public class WinnerHandler : AbstractHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public WinnerHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public override async Task<List<Rate>> Handle(List<Rate> rates)
    {
        switch (rates.Count)
        {
            case 0:
                return rates;

            case 1:
            {
                var rate = rates.First();
                rate.IsClosed = true;
                rate.Payout = rate.Amount;
                return rates;
            }
        }

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var currState = await db.CurrencyStates
            .FirstOrDefaultAsync(currState => currState.CurrencyId == rates.First().CurrencyId);

        foreach (var rate in rates)
        {
            rate.IsWon = rate.RateCurrencyExchange == currState.CurrencyExchangeRate;
            rate.IsClosed = true;
        }

        return await base.Handle(rates);
    }
}
