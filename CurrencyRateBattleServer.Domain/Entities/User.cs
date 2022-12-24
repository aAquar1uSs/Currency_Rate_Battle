using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public sealed class User
{
    public UserId Id { get; set; }

    public Email Email { get; }

    public Password Password { get; }

    private User(UserId id,
        Email email,
        Password password)
    {
        Id = id;
        Email = email;
        Password = password;
    }

    public static Result<User> TryCreate(Guid id, string email, string password)
    {
        var oneIdResult = UserId.TryCreate(id);
        if (oneIdResult.IsFailure)
            return Result.Failure<User>(oneIdResult.Error);

        var emailResult = Email.TryCreate(email);
        if (emailResult.IsFailure)
            return Result.Failure<User>(emailResult.Error);

        var passwordResult = Password.TryCreate(password);
        if (passwordResult.IsFailure)
            return Result.Failure<User>(passwordResult.Error);

        return new User(oneIdResult.Value, emailResult.Value, passwordResult.Value);
    }

    public static User Create(Guid id, string email, string password)
    {
        var userId = UserId.Create(id);

        var emailDomain = Email.Create(email);

        var passwordDomain = Password.Create(password);

        return new User(userId, emailDomain, passwordDomain);
    }
}
