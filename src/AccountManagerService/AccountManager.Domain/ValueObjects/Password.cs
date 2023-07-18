using AccountManager.Domain.Infrastructure.Encoder;
using CSharpFunctionalExtensions;

namespace AccountManager.Domain.ValueObjects;

public class Password
{
    public string Value { get; }

    private Password(string value)
    {
        Value = value;
    }

    public static Result<Password> TryCreate(string password)
    {
        if(!IsValidPassword(password))
            return Result.Failure<Password>("Password incorrect");
        var encodedPassword = Sha256Encoder.Encrypt(password);

        return new Password(encodedPassword);
    }

    public static Password Create(string password) => new(Sha256Encoder.Encrypt(password));
    
    private static bool IsValidPassword(string password)
    {
        return password.Length is < 30 and > 6;
    }
}