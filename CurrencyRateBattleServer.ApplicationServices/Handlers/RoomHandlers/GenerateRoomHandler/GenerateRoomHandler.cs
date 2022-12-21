using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GenerateRoomHandler;

public class GenerateRoomHandler : IRequestHandler<GenerateRoomCommand>
{
    private readonly ILogger<GenerateRoomHandler> _logger;
    private readonly IRoomRepository _roomRepository;

    public GenerateRoomHandler(ILogger<GenerateRoomHandler> logger, IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    //Add settings to count of rooms can be generated
    public async Task<Unit> Handle(GenerateRoomCommand request, CancellationToken cancellationToken)
    {
        await _roomRepository.CreateAsync(cancellationToken);
        return Unit.Value;
    }
}
