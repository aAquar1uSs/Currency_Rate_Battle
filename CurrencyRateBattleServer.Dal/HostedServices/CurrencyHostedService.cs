using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.HostedServices;

public class CurrencyHostedService : IHostedService, IDisposable
{
    private readonly ILogger<CurrencyHostedService> _logger;

    private Timer? _timer;

    private static readonly object _sync = new();

    private readonly ICurrencyStateRepository _currencyStateRepository;

    public CurrencyHostedService(ILogger<CurrencyHostedService> logger,
        ICurrencyStateRepository currencyStateRepository)
    {
        _logger = logger;
        _currencyStateRepository = currencyStateRepository;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Currency Hosted Service running.");

        _timer = new Timer(SyncCallback, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(30));

        return Task.CompletedTask;
    }

    private void SyncCallback(object? state)
    {
        try
        {
            if (!Monitor.TryEnter(_sync))
                return;
            else
                Callback(state);
        }
        finally
        {
            if (Monitor.IsEntered(_sync))
                Monitor.Exit(_sync);
        }
    }

    private async void Callback(object? state)
    {
        _logger.LogInformation("PrepareUpdateCurrencyRateAsync has been invoked.");
        await _currencyStateRepository.PrepareUpdateCurrencyRateAsync();
        _logger.LogInformation("PrepareUpdateCurrencyRateAsync сompleted the execution.");
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
