using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;

public class GetRoomCommand : IRequest<Result<GetRoomResponse>>
{
    public GetRoomCommand(bool isClosed)
    {
        IsClosed = isClosed;
    }
    
    public bool IsClosed { get; }
}
