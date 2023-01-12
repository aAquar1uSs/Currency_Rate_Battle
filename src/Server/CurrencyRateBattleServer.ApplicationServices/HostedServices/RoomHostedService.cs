using CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GenerateRoomHandler;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.HostedServices;

public class RoomHostedService : IHostedService, IDisposable
{
    private readonly ILogger<RoomHostedService> _logger;
    private Timer? _timer;
    private readonly IServiceScopeFactory _scopeFactory;

    public RoomHostedService(ILogger<RoomHostedService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
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
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = new GenerateRoomCommand();
        _ = await mediator.Send(command);
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
