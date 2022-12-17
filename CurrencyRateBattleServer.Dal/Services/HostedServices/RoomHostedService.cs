using CurrencyRateBattleServer.Dal.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services.HostedServices;

public class RoomHostedService : IHostedService, IDisposable
{
    private readonly ILogger<RoomHostedService> _logger;

    private Timer? _timer;

    private readonly IRoomRepository _roomRepository;

    public RoomHostedService(ILogger<RoomHostedService> logger,
        IRoomRepository roomRepository)
    {
        _logger = logger;
        _roomRepository = roomRepository;
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
        _logger.LogInformation("GenerateRoomsByCurrencyCountAsync has been invoked.");
        await _roomRepository.GenerateRoomsByCurrencyCountAsync();
        _logger.LogInformation("GenerateRoomsByCurrencyCountAsync сompleted the execution.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _ = (_timer?.Change(Timeout.Infinite, 0));
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}
