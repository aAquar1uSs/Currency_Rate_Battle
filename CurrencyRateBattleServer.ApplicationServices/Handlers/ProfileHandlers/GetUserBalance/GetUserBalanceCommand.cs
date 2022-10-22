using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetUserBalance;

public class GetUserBalanceCommand : IRequest<Result<GetUserBalanceResponse>>
{
    public Guid? UserId { get; set; }
}
