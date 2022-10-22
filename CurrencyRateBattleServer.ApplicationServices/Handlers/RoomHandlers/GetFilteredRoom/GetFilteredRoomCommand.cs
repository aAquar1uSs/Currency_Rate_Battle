using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetFilteredRoom;

public class GetFilteredRoomCommand : IRequest<Result<GetFilteredRoomResponse>>
{
    public FilterDto Filter { get; set; }
}
