using System.Net.Mail;
using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> TryCreate(string email)
    {
        if (!IsValidEmail(email))
            return Result.Failure<Email>("Invalid email.");

        return new Email(email);
    }

    public static Email Create(string email) => new Email(email);

    private static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
            return false;

        if (trimmedEmail.Length is > 30 or < 6)
            return false;

        try {
            var addr = new MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch {
            return false;
        }
    }
}
