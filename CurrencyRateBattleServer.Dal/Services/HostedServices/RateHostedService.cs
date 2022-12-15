﻿using CurrencyRateBattleServer.Dal.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services.HostedServices;

public class RateHostedService : IHostedService, IDisposable
{
    private Timer? _timer;

    private static readonly object _sync = new();

    private readonly ILogger<RateHostedService> _logger;

    private readonly IRoomRepository _roomRepository;

    public RateHostedService(ILogger<RateHostedService> logger,
        IRoomRepository roomRepository)
    {
        _logger = logger;
        _roomRepository = roomRepository;
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
        await _roomRepository.CheckRoomsStateAsync();
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
