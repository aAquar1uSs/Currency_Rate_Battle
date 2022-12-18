﻿using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using NbuClient;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.UpdateCurrencyRateHandlers;

public class UpdateCurrencyRateHandler : IRequestHandler<UpdateCurrencyRateCommand>
{
    private readonly ILogger<UpdateCurrencyRateHandler> _logger;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly NbuApiClient _nbuApiClient;
    private readonly ICurrencyStateRepository _currencyStateRepository;

    public UpdateCurrencyRateHandler(ILogger<UpdateCurrencyRateHandler> logger,
        ICurrencyRepository currencyRepository, NbuApiClient client, ICurrencyStateRepository currencyStateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
        _nbuApiClient = client ?? throw new ArgumentNullException(nameof(client));
        _currencyStateRepository = currencyStateRepository ?? throw new ArgumentNullException(nameof(currencyStateRepository));
    }
    
    public async Task<Unit> Handle(UpdateCurrencyRateCommand request, CancellationToken cancellationToken)
    {
        var updatedCurrencies = await _nbuApiClient.GetCurrencyRatesAsync(cancellationToken);
        var currencies = updatedCurrencies.Select(x => x.ToDomain()).ToArray();

        await _currencyRepository.UpdateAsync(currencies, cancellationToken);

        foreach (var currency in currencies)
        {
            await _currencyStateRepository.UpdateCurrencyRateAsync(currency, cancellationToken);
        }

        return Unit.Value;
    }
}
