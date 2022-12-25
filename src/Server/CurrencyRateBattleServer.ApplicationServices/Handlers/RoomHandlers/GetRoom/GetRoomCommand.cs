using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;

public class GetRoomCommand : IRequest<Result<GetRoomResponse>>
{
    public bool IsClosed { get; set; }
}
