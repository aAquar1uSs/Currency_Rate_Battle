using CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GenerateRoomHandler;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.HostedServices;

public class RoomHostedService : IHostedService, IDisposable
{
    private readonly ILogger<RoomHostedService> _logger;
    private readonly IMediator _mediator;
    private Timer? _timer;

    public RoomHostedService(ILogger<RoomHostedService> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
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
        var command = new GenerateRoomCommand();
        _ = await _mediator.Send(command);
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
