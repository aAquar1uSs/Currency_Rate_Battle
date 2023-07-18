using AccountManager.Domain;
using AccountManager.Domain.Errors;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountManager.ApplicationServices.Handlers.Registration;

public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, Result<Tokens, Error>>
{
    public Task<Result<Tokens, Error>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
