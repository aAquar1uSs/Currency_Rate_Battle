using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.HostedServices.Handlers.Interface;

namespace CurrencyRateBattleServer.Services.HostedServices.Handlers;

public abstract class AbstractHandler : IHandler
{
    private IHandler _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        this._nextHandler = handler;

        return handler;
    }

    public virtual Task<List<Rate>> Handle(List<Rate> rates)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.Handle(rates);
        }

        return null;
    }
}
