using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Services.Interfaces;


namespace CurrencyRateBattleServer.Services.HostedServices;

public class RoomHostedService : IHostedService, IDisposable
{
    private readonly ILogger<CurrencyHostedService> _logger;

    private Timer? _timer;

    private readonly IRoomService _roomService;

    private readonly IServiceScopeFactory _scopeFactory;

    public RoomHostedService(ILogger<CurrencyHostedService> logger,
        IRoomService roomService, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _roomService = roomService;
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
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        foreach (var curr in dbContext.Currencies)
        {
            await _roomService.CreateRoomAsync(dbContext, curr);
        }

        _ = await dbContext.SaveChangesAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
