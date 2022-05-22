using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Models;
using static System.DateTime;

namespace CurrencyRateBattleServer.Services.HostedServices;

public class RoomHostedService : IHostedService, IDisposable
{
    private readonly ILogger<CurrencyHostedService> _logger;

    private Timer _timer;

    private readonly IServiceScopeFactory _scopeFactory;


    public RoomHostedService(ILogger<CurrencyHostedService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Room Hosted Service running.");

        _timer = new Timer(Callback, null, TimeSpan.Zero,
            TimeSpan.FromHours(4));

        return Task.CompletedTask;
    }

    private async void Callback(object? state)
    {
        var currentDate = ParseExact(UtcNow.ToString("MM.dd.yyyy HH:00:00"),
            "MM.dd.yyyy HH:mm:ss", null);

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        //Todo Transfer to method
        foreach (var curr in dbContext.Currencies)
        {
            var currState = new CurrencyState
            {
                Date = currentDate,
                CurrencyExchangeRate = 0,
                Currency = curr,
                CurrencyId = curr.Id,
                Room = new Room {Date = currentDate.AddDays(1), IsClosed = false}
            };

            await dbContext.CurrencyStates.AddAsync(currState);
        }

        await dbContext.SaveChangesAsync();
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
