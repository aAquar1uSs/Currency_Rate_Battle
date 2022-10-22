using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;

public class CreateHistoryCommand : IRequest<Result<CreateHistoryResponse>>
{
    public Guid? UserId { get; set; }
    
    public AccountHistoryDto AccountHistory { get; set; }
}
