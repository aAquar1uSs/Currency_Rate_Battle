using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetRates;

public class GetRatesCommand : IRequest<Result<GetRatesResponse>>
{
    public bool? IsActive { get; set; }

    public string? CurrencyName { get; set; }

}
