using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.HostedServices.Handlers.Interfaces;

public interface IHandler
{
    IHandler SetNext(IHandler handler);

    Task<List<Rate>>? Handle(List<Rate> rate);
}
