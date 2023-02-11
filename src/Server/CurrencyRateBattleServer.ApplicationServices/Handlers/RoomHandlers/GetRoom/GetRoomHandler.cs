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
    private readonly IRoomQueryRepository _roomQueryRepository;

    public GetRoomHandler(ILogger<GetRoomHandler> logger, IRoomQueryRepository roomQueryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roomQueryRepository = roomQueryRepository ?? throw new ArgumentNullException(nameof(roomQueryRepository));
    }

    public async Task<Result<GetRoomResponse>> Handle(GetRoomCommand request, CancellationToken cancellationToken)
    { 
        _logger.LogDebug($"{nameof(GetRoomHandler)} was caused. Start processing.");

        var rooms = await _roomQueryRepository.FindAsync(request.IsClosed, cancellationToken);

        return new GetRoomResponse {Rooms = rooms.ToDto()};
    }
}
