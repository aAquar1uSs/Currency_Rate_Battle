using CurrencyRateBattleServer.Dal.Entities;

namespace CurrencyRateBattleServer.Services.HostedServices.Handlers.Interfaces;

public interface IHandler
{
    IHandler SetNext(IHandler handler);

    Task<List<RateDal>>? Handle(List<RateDal> rates);
}
