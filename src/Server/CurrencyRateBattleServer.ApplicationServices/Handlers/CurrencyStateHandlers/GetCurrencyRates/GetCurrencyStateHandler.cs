using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;

public class GetCurrencyStateHandler : IRequestHandler<GetCurrencyStateCommand, Result<GetCurrencyStateResponse>>
{
    private readonly ILogger<GetCurrencyStateHandler> _logger;
    private readonly ICurrencyRepository _currencyRepository;

    public GetCurrencyStateHandler(ILogger<GetCurrencyStateHandler> logger, ICurrencyRepository currencyRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
    }

    public async Task<Result<GetCurrencyStateResponse>> Handle(GetCurrencyStateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateHandler)} was caused. Start processing.");

        var currencyRates = await _currencyRepository.GetAsync(cancellationToken);

        return new GetCurrencyStateResponse { CurrencyStates = currencyRates.ToDto()};
    }
}
