using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Services.Interfaces;


namespace CurrencyRateBattleServer.Services.HostedServices;

public class CurrencyHostedService : IHostedService, IDisposable
{
    private readonly ILogger<CurrencyHostedService> _logger;

    private Timer _timer;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ICurrencyStateService _currencyStateService;

    public CurrencyHostedService(ILogger<CurrencyHostedService> logger,
        IServiceScopeFactory scopeFactory,
        ICurrencyStateService currencyStateService)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _currencyStateService = currencyStateService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Currency Hosted Service running.");

        _timer = new Timer(Callback, null, TimeSpan.FromSeconds(10),
            TimeSpan.FromMinutes(10));

        return Task.CompletedTask;
    }

    private async void Callback(object? state)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await _currencyStateService.GetCurrencyRateByNameFromNbuApiAsync();

        foreach (var currState in dbContext.CurrencyStates)
        {
            await _currencyStateService.UpdateCurrencyRateByIdAsync(currState.CurrencyId);
        }
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
