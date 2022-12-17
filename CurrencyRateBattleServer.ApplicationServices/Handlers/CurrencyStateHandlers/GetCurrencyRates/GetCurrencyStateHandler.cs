using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;

public class GetCurrencyStateHandler : IRequestHandler<GetCurrencyStateCommand, Result<GetCurrencyStateResponse>>
{
    private readonly ILogger<GetCurrencyStateHandler> _logger;

    private readonly ICurrencyStateRepository _currencyStateRepository;

    public GetCurrencyStateHandler(ILogger<GetCurrencyStateHandler> logger, ICurrencyStateRepository currencyStateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyStateRepository = currencyStateRepository ?? throw new ArgumentNullException(nameof(currencyStateRepository));
    }

    public async Task<Result<GetCurrencyStateResponse>> Handle(GetCurrencyStateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetCurrencyStateHandler)} was caused. Start processing.");

        var currencyRates = await _currencyStateRepository.GetCurrencyStateAsync();

        return new GetCurrencyStateResponse { CurrencyStates = currencyRates.ToDto()};
    }
}
