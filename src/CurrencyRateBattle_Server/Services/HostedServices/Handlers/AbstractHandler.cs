using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.HostedServices.Handlers.Interfaces;

namespace CurrencyRateBattleServer.Services.HostedServices.Handlers;

public abstract class AbstractHandler : IHandler
{
    private IHandler _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;

        return handler;
    }

    public virtual Task<List<Rate>> Handle(List<Rate> rates)
    {
        return _nextHandler?.Handle(rates);
    }
}
