using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetUserBalance;

public class GetUserBalanceCommand : IRequest<Result<GetUserBalanceResponse>>
{
    public string UserId { get; set; }
}
