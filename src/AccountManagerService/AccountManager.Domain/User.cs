using AccountManager.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace AccountManager.Domain;

public sealed class User
{
    public Email Email { get; }

    public Password Password { get; }

    private User(Email email,
        Password password)
    {
        Email = email;
        Password = password;
    }

    public static Result<User> TryCreate(string email, string password)
    {
        var emailResult = Email.TryCreate(email);
        if (emailResult.IsFailure)
            return Result.Failure<User>(emailResult.Error);

        var passwordResult = Password.TryCreate(password);
        if (passwordResult.IsFailure)
            return Result.Failure<User>(passwordResult.Error);

        return new User(emailResult.Value, passwordResult.Value);
    }

    public static User Create(string email, string password)
    {
        var emailDomain = Email.Create(email);

        var passwordDomain = Password.Create(password);

        return new User(emailDomain, passwordDomain);
    }
}