using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GetRoom;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetRates;

public class GetRatesHandler : IRequestHandler<GetRoomCommand, Result<GetRoomResponse>>
{
    public Task<Result<GetRoomResponse>> Handle(GetRoomCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
