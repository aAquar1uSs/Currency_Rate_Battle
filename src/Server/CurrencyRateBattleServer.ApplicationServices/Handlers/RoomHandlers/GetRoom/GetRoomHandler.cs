using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;

//ToDo Maybe make new method in Rate Repository for calculating count of bets
public class GetRoomHandler : IRequestHandler<GetRoomCommand, Result<GetRoomResponse>>
{
    private readonly ILogger<GetRoomHandler> _logger;
    private readonly IRoomRepository _roomRepository;

    public GetRoomHandler(ILogger<GetRoomHandler> logger, IRoomRepository roomRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
    }

    public async Task<Result<GetRoomResponse>> Handle(GetRoomCommand request, CancellationToken cancellationToken)
    { 
        _logger.LogDebug($"{nameof(GetRoomHandler)} was caused. Start processing.");

        var rooms = await _roomRepository.FindAsync(request.IsClosed, cancellationToken);

        var roomDto = rooms.Select(x => x.ToDto())
            .ToArray();

        return new GetRoomResponse { Rooms = roomDto };
    }
}
