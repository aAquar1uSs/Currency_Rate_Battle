using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Helpers;
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
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        if (rates.Count <= 1)
            throw new CustomException();

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
