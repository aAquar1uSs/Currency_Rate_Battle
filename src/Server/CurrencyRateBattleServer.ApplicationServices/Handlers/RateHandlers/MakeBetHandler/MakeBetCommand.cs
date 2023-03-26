using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;

public class MakeBetCommand : IRequest<Result<MakeBetResponse, Error>>
{
    public MakeBetCommand(string userEmail, UserRateDto userRateToCreate)
    {
        UserEmail = userEmail;
        UserRateToCreate = userRateToCreate;
    }
    
    public string UserEmail { get; set; }
    
    public UserRateDto UserRateToCreate { get; set; }
}
