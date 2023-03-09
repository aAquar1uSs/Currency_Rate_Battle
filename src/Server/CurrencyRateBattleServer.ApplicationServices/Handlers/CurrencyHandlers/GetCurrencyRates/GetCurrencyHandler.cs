using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyHandlers.GetCurrencyRates;

public class GetCurrencyHandler : IRequestHandler<GetCurrencyCommand, Result<GetCurrencyResponse>>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetCurrencyHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
    }

    public async Task<Result<GetCurrencyResponse>> Handle(GetCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currencyRates = await _currencyRepository.Get(cancellationToken);

        return new GetCurrencyResponse { CurrencyStates = currencyRates.Select(x => x.ToDto()).ToArray()};
    }
}
