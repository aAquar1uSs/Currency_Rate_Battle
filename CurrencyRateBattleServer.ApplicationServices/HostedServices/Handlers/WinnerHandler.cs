using CurrencyRateBattleServer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRateBattleServer.Dal.Services.HostedServices.Handlers;

public class WinnerHandler : AbstractHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public WinnerHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public override async Task<List<RateDal>> Handle(List<RateDal> rates)
    {
        if (rates.Count == 1)
        {
            var rate = rates.First();
            rate.IsClosed = true;
            rate.Payout = rate.Amount;
            return rates;
        }

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var currState = await db.CurrencyStates
            .FirstOrDefaultAsync(curr => curr.Room.Id == rates.First().Room.Id);

        foreach (var rate in rates)
        {
            rate.IsWon = currState != null && rate.RateCurrencyExchange == currState.CurrencyExchangeRate;
            rate.IsClosed = true;
        }

        return await base.Handle(rates);
    }
}
