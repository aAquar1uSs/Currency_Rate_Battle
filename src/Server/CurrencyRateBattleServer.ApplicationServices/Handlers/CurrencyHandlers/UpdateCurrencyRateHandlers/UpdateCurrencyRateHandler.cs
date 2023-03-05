using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using NbuClient;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyHandlers.UpdateCurrencyRateHandlers;

public class UpdateCurrencyRateHandler : IRequestHandler<UpdateCurrencyRateCommand>
{
    private readonly ILogger<UpdateCurrencyRateHandler> _logger;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly INbuApiClient _nbuApiApiClient;

    public UpdateCurrencyRateHandler(ILogger<UpdateCurrencyRateHandler> logger,
        ICurrencyRepository currencyRepository, INbuApiClient apiClient)
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

        if (currencies.Any(x => x == null))
        {
            _logger.LogWarning("Invalid data in response from NBU api. Skip processing");
            return Unit.Value;
        }

        var availableCurrenciesIds = await _currencyRepository.GetAsync(cancellationToken);
        

        foreach (var currency in availableCurrenciesIds)
        {
            var currencyToUpdate = updatedCurrencies.FirstOrDefault(x => x.Currency == currency.CurrencyName.Value);
            if (currencyToUpdate is null)
                continue;

            var updatedResult = currency.TryUpdateCurrency(currencyToUpdate.Rate, currencyToUpdate.Date);
            if (updatedResult.IsFailure)
            {
                _logger.LogError(updatedResult.Error);
                continue;    
            }
            
            await _currencyRepository.UpdateAsync(currency, cancellationToken);   
        }

        return Unit.Value;
    }
}
