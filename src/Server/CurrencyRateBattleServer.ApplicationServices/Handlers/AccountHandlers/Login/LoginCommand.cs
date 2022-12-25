using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Login;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public UserDto UserDto { get; set; }
}
