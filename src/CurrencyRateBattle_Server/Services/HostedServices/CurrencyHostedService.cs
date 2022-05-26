using CurrencyRateBattleServer.Services.Interfaces;


namespace CurrencyRateBattleServer.Services.HostedServices;

public class CurrencyHostedService : IHostedService, IDisposable
{
    private readonly ILogger<CurrencyHostedService> _logger;

    private Timer? _timer;

    private readonly ICurrencyStateService _currencyStateService;

    public CurrencyHostedService(ILogger<CurrencyHostedService> logger,
        ICurrencyStateService currencyStateService)
    {
        _logger = logger;
        _currencyStateService = currencyStateService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Currency Hosted Service running.");

        _timer = new Timer(Callback, null, TimeSpan.FromSeconds(8),
            TimeSpan.FromMinutes(10));

        return Task.CompletedTask;
    }

    private async void Callback(object? state)
    {
        await _currencyStateService.PrepareUpdateCurrencyRateAsync();
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
