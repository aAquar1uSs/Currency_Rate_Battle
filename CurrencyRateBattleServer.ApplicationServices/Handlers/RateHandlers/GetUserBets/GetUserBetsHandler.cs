using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;

public class GetUserBetsHandler : IRequestHandler<GetUserBetsCommand, Result<GetUserBetsResponse>>
{
    public Task<Result<GetUserBetsResponse>> Handle(GetUserBetsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
