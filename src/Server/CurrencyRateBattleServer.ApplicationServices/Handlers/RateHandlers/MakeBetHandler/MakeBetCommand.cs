using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dto;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;

public class MakeBetCommand : IRequest<Result<MakeBetResponse>>
{
    public string UserId { get; set; }
    
    public UserRateDto UserRateToCreate { get; set; }
}
