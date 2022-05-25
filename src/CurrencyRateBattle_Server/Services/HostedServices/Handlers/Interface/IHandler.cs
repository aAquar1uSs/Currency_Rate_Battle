using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.HostedServices.Handlers.Interface;

public interface IHandler
{
    IHandler SetNext(IHandler handler);

    Task<List<Rate>> Handle(List<Rate> rate);
}
