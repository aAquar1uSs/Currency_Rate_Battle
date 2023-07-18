using AccountManager.Domain;
using AccountManager.Domain.Errors;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountManager.ApplicationServices.Handlers.Login;

public class LoginCommand : IRequest<Result<Tokens, Error>>
{
    public LoginCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
    
    public string Email { get; }
    
    public string Password { get; }
}
