using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetUserBalance;

public class GetUserBalanceCommand : IRequest<Result<GetUserBalanceResponse, Error>>
{
    public string UserId { get; set; }
}
