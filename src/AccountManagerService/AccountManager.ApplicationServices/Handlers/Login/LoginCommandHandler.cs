using AccountManager.Domain;
using AccountManager.Domain.Errors;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountManager.ApplicationServices.Handlers.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<Tokens, Error>>
{
    public Task<Result<Tokens, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
