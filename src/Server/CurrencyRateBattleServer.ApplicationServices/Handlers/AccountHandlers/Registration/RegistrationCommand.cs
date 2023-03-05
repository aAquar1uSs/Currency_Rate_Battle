using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Registration;

public class RegistrationCommand : IRequest<Result<RegistrationResponse, Error>>
{
    public string Email { get; set; }

    public string Password { get; set; }
}
