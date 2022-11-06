using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetFilteredRoom;

public class GetFilteredRoomHandler : IRequestHandler<GetFilteredRoomCommand, Result<GetFilteredRoomResponse>>
{
    private readonly ILogger<GetFilteredRoomHandler> _logger;

    private readonly IRoomService _roomService;

    public GetFilteredRoomHandler(ILogger<GetFilteredRoomHandler> logger, IRoomService roomService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
    }

    public async Task<Result<GetFilteredRoomResponse>> Handle(GetFilteredRoomCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetFilteredRoomHandler)} was caused.");

        var rooms = await _roomService.GetActiveRoomsWithFilterAsync(request.Filter);

        if (rooms is null)
            return Result.Failure<GetFilteredRoomResponse>("");

        return new GetFilteredRoomResponse {Rooms = rooms.ToDto()};
    }
}
