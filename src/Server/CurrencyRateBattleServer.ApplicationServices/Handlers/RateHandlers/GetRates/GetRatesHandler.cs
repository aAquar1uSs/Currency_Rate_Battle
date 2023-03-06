using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetRates;

public class GetRatesHandler : IRequestHandler<GetRatesCommand, Result<GetRatesResponse>>
{
    private readonly IRateRepository _rateRepository;

    public GetRatesHandler(IRateRepository rateRepository)
    {
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
    }

    public async Task<Result<GetRatesResponse>> Handle(GetRatesCommand request, CancellationToken cancellationToken)
    {
        var rates = await _rateRepository.FindAsync(request.IsActive, request.CurrencyName, cancellationToken);

        return new GetRatesResponse { Rates = rates.Select(x => x.ToDto()).ToArray() };
    }
}
