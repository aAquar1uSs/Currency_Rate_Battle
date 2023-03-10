using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;

public class GetRoomHandler : IRequestHandler<GetRoomCommand, Result<GetRoomResponse>>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomHandler(ILogger<GetRoomHandler> logger, IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
    }

    public async Task<Result<GetRoomResponse>> Handle(GetRoomCommand request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.Find(request.IsClosed, cancellationToken);

        var roomDto = rooms.Select(x => x.ToDto()).ToArray();

        return new GetRoomResponse { Rooms = roomDto };
    }
}
