using AccountManager.Domain;
using AccountManager.Domain.Errors;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountManager.ApplicationServices.Handlers.Registration;

public class RegistrationCommand : IRequest<Result<Tokens, Error>>
{
    public RegistrationCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
    
    public string Email { get; }
    
    public string Password { get; }
}
