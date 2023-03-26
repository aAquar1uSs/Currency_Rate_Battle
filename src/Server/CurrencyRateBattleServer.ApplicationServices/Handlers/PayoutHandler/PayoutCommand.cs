using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.PayoutHandler;

public class PayoutCommand : IRequest<Maybe<Error>>
{
    public PayoutCommand(Guid accountId, decimal payout, Guid roomId)
    {
        AccountId = accountId;
        Payout = payout;
        RoomId = roomId;
    }
    
    public Guid AccountId { get; }
    
    public decimal Payout { get; }
    
    public Guid RoomId { get; }
}
