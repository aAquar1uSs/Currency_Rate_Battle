using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public sealed class User
{
    public UserId Id { get; set; }

    public Email Email { get; }

    public Password Password { get; }

    public AccountId? AccountId { get; }

    private User(UserId id,
        Email email,
        Password password,
        AccountId? accountId)
    {
        Id = id;
        Email = email;
        Password = password;
        AccountId = accountId;
    }

    public static Result<User> TryCreate(Guid id, string email, string password, Guid? accountId)
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

        if (accountId is null)
            return new User(oneIdResult.Value, emailResult.Value, passwordResult.Value, null);

        var accOneIdResult = ValueObjects.AccountId.TryCreate((Guid)accountId);
        if (accOneIdResult.IsFailure)
            return Result.Failure<User>(accOneIdResult.Error);
        
        return new User(oneIdResult.Value, emailResult.Value, passwordResult.Value, accOneIdResult.Value);
    }

    public static User Create(Guid id, string email, string password, Guid? accountId)
    {
        var userId = UserId.Create(id);

        var emailDomain = Email.Create(email);

        var passwordDomain = Password.Create(password);

        if (accountId is null)
            return new User(userId, emailDomain, passwordDomain, null);

        var accOneId = ValueObjects.AccountId.Create((Guid)accountId);

        return new User(userId, emailDomain, passwordDomain, accOneId);
    }
}
