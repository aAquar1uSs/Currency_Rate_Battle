using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetFilteredRoom;

public class GetFilteredRoomHandler : IRequestHandler<GetFilteredRoomCommand, Result<GetFilteredRoomResponse>>
{
    private readonly IRoomQueryRepository _roomQueryRepository;

    public GetFilteredRoomHandler(IRoomQueryRepository roomQueryRepository)
    {
        _roomQueryRepository = roomQueryRepository ?? throw new ArgumentNullException(nameof(roomQueryRepository));
    }

    public async Task<Result<GetFilteredRoomResponse>> Handle(GetFilteredRoomCommand request, CancellationToken cancellationToken)
    {
        var filter = new Filter(request.Filter.CurrencyName, request.Filter.StartDate, request.Filter.EndDate);
        
        var rooms = await _roomQueryRepository.GetActiveRoomsWithFilterAsync(filter, cancellationToken);

        return new GetFilteredRoomResponse { Rooms = rooms.Select(x => x.ToDto()).ToArray() };
    }
}
