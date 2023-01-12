using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Dto;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Registration;

public class RegistrationCommand : IRequest<Result<RegistrationResponse>>
{
    public UserDto UserDto { get; set; }
}
