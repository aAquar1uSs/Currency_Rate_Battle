using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;

public class GetAccountHistoryCommand : IRequest<Result<GetAccountHistoryResponse>>
{
    public Guid? UserId { get; set; }
}
