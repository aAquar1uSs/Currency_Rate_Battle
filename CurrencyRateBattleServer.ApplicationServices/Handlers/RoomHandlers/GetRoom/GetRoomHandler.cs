using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;

public class GetRoomHandler : IRequestHandler<GetRoomCommand, Result<GetRoomResponse>>
{
    private readonly ILogger<GetRoomHandler> _logger;

    private readonly IRoomService _roomService;

    public GetRoomHandler(ILogger<GetRoomHandler> logger, IRoomService roomService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    }

    public async Task<Result<GetRoomResponse>> Handle(GetRoomCommand request, CancellationToken cancellationToken)
    {
        //ToDo Create method which been get CountRate etc...
        _logger.LogDebug($"{nameof(GetRoomHandler)} was caused. Start processing.");

        var rooms = await _roomService.GetRoomsAsync(request.IsClosed);

        return new GetRoomResponse {Rooms = rooms.ToDto()};
    }
}
