using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyHandlers.GetCurrencyRates;

public class GetCurrencyCommand : IRequest<Result<GetCurrencyResponse>>
{
    
}
