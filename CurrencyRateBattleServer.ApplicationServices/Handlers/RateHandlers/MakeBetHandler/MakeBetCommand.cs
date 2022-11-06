using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dto;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;

public class MakeBetCommand : IRequest<Result<MakeBetResponse>>
{
    public Guid UserId { get; set; }
    
    public RateDto RateToCreate { get; set; }
}
