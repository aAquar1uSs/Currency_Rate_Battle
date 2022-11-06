using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;

public class GetCurrencyStateHandler : IRequestHandler<GetCurrencyStateCommand, Result<GetCurrencyStateResponse>>
{
    private readonly ILogger<GetCurrencyStateHandler> _logger;

    private readonly ICurrencyStateService _currencyStateService;

    public GetCurrencyStateHandler(ILogger<GetCurrencyStateHandler> logger, ICurrencyStateService currencyStateService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyStateService = currencyStateService ?? throw new ArgumentNullException(nameof(currencyStateService));
    }

    public async Task<Result<GetCurrencyStateResponse>> Handle(GetCurrencyStateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateHandler)} was caused. Start processing.");

        var currencyRates = await _currencyStateService.GetCurrencyStateAsync();

        return new GetCurrencyStateResponse { CurrencyStates = currencyRates.ToArray().ToDto()};
    }
}
