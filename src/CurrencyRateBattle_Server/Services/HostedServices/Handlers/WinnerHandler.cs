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
            .FirstOrDefaultAsync(curr => curr.RoomId == rates.First().RoomId);

        foreach (var rate in rates)
        {
            rate.IsWon = currState != null && rate.RateCurrencyExchange == currState.CurrencyExchangeRate;
            rate.IsClosed = true;
        }

        return await Handle(rates);
    }
}
