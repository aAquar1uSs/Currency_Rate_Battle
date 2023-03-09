using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;

public class GetUserBetsCommand : IRequest<Result<GetUserBetsResponse, Error>>
{
    public GetUserBetsCommand(string userEmail)
    {
        UserEmail = userEmail;
    }
    
    public string UserEmail { get; }
}
