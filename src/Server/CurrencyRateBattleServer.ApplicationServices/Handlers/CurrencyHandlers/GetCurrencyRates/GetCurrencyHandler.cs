using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyHandlers.GetCurrencyRates;

public class GetCurrencyHandler : IRequestHandler<GetCurrencyCommand, Result<GetCurrencyResponse>>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetCurrencyHandler(ILogger<GetCurrencyHandler> logger, ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
    }

    public async Task<Result<GetCurrencyResponse>> Handle(GetCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currencyRates = await _currencyRepository.GetAsync(cancellationToken);

        return new GetCurrencyResponse { CurrencyStates = currencyRates.ToDto()};
    }
}
