﻿using CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.CalculationRateHandler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.HostedServices;

public class RateHostedService : IHostedService, IDisposable
{
    private Timer? _timer;
    private static readonly object _sync = new();
    private readonly ILogger<RateHostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public RateHostedService(ILogger<RateHostedService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rate Hosted Service running.");

        _timer = new Timer(SyncCallback, null, TimeSpan.FromMinutes(1),
            TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }

    private void SyncCallback(object? state)
    {
        try
        {
            if (!Monitor.TryEnter(_sync)) return;
            else Callback(state);
        }
        finally
        {
            if (Monitor.IsEntered(_sync))
                Monitor.Exit(_sync);
        }
    }

    private async void Callback(object? state)
    {
        _logger.LogInformation("CheckRoomsStateAsync has been invoked.");
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = new CalculationRateCommand();
        _ = await mediator.Send(command);
        _logger.LogInformation("CheckRoomsStateAsync сompleted the execution.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_timer != null)
            _ = _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_timer != null)
            _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
