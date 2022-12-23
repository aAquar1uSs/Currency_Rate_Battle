using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using NbuClient;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.UpdateCurrencyRateHandlers;

public class UpdateCurrencyRateHandler : IRequestHandler<UpdateCurrencyRateCommand>
{
    private readonly ILogger<UpdateCurrencyRateHandler> _logger;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly INbuApiClient _nbuApiApiClient;

    public UpdateCurrencyRateHandler(ILogger<UpdateCurrencyRateHandler> logger,
        ICurrencyRepository currencyRepository, INbuApiClient apiClient, ICurrencyStateRepository currencyStateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
        _nbuApiApiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task<Unit> Handle(UpdateCurrencyRateCommand request, CancellationToken cancellationToken)
    {
        var updatedCurrencies = await _nbuApiApiClient.GetCurrencyRatesAsync(cancellationToken);
        if (updatedCurrencies is null)
        {
            _logger.LogWarning("Failed to get currency data form NBU api. Skip processing.");
            return Unit.Value;
        }

        var currencies = updatedCurrencies.Select(x => x.ToDomain()).ToArray();

        var availableCurrenciesIds = await _currencyRepository.GetAllIds(cancellationToken);

        var currenciesToUpdate = currencies.Select(x => x)
            .Where(x => availableCurrenciesIds.Contains(x.CurrencyName?.Value))
            .ToArray();

        await _currencyRepository.UpdateAsync(currenciesToUpdate, cancellationToken);

        return Unit.Value;
    }
}
