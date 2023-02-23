using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;

public class GetCurrencyHandler : IRequestHandler<GetCurrencyCommand, Result<GetCurrencyResponse>>
{
    private readonly ILogger<GetCurrencyHandler> _logger;
    private readonly ICurrencyRepository _currencyRepository;

    public GetCurrencyHandler(ILogger<GetCurrencyHandler> logger, ICurrencyRepository currencyRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
    }

    public async Task<Result<GetCurrencyResponse>> Handle(GetCurrencyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyHandler)} was caused. Start processing.");

        var currencyRates = await _currencyRepository.GetAsync(cancellationToken);

        return new GetCurrencyResponse { CurrencyStates = currencyRates.ToDto()};
    }
}
