using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Services.HostedServices.Handlers.Interfaces;

namespace CurrencyRateBattleServer.Dal.Services.HostedServices.Handlers;

public abstract class AbstractHandler : IHandler
{
    private IHandler? _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;

        return handler;
    }

    public virtual Task<List<RateDal>>? Handle(List<RateDal> rates)
    {
        return _nextHandler?.Handle(rates);
    }
}
