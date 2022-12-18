using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetFilteredRoom;

public class GetFilteredRoomHandler : IRequestHandler<GetFilteredRoomCommand, Result<GetFilteredRoomResponse>>
{
    private readonly ILogger<GetFilteredRoomHandler> _logger;
    private readonly IRoomQueryRepository _roomQueryRepository;

    public GetFilteredRoomHandler(ILogger<GetFilteredRoomHandler> logger, IRoomQueryRepository roomQueryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roomQueryRepository = roomQueryRepository ?? throw new ArgumentNullException(nameof(roomQueryRepository));
    }

    public async Task<Result<GetFilteredRoomResponse>> Handle(GetFilteredRoomCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetFilteredRoomHandler)} was caused.");
        var filter = new Filter(request.Filter.CurrencyName, request.Filter.StartDate, request.Filter.EndDate);
        
        var rooms = await _roomQueryRepository.GetActiveRoomsWithFilterAsync(filter, cancellationToken);

        return new GetFilteredRoomResponse {Rooms = rooms.ToDto()};
    }
}
