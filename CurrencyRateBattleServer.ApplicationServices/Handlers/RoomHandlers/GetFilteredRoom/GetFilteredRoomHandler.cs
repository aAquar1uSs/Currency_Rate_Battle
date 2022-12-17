using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetFilteredRoom;

public class GetFilteredRoomHandler : IRequestHandler<GetFilteredRoomCommand, Result<GetFilteredRoomResponse>>
{
    private readonly ILogger<GetFilteredRoomHandler> _logger;

    private readonly IRoomRepository _roomRepository;

    public GetFilteredRoomHandler(ILogger<GetFilteredRoomHandler> logger, IRoomRepository roomRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
    }

    public async Task<Result<GetFilteredRoomResponse>> Handle(GetFilteredRoomCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetFilteredRoomHandler)} was caused.");

        var rooms = await _roomRepository.GetActiveRoomsWithFilterAsync(request.Filter);

        if (rooms is null)
            return Result.Failure<GetFilteredRoomResponse>("");

        return new GetFilteredRoomResponse {Rooms = rooms.ToDto()};
    }
}
