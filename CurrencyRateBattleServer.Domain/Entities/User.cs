using System.Net.Mail;
using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Infrastructure.Encoder;

namespace CurrencyRateBattleServer.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public Account Account { get; set; }

    public static Result<User> Create(string email, string password, Account account = null!)
    {
        var validateResult = Validate(email, password);

        if (validateResult.IsFailure)
            return Result.Failure<User>("Invalid data. Please try again.");

        var encodedPassword = Sha256Encoder.Encrypt(password);

        return new User { Account = account, Email = email, Password = encodedPassword };
    }

    private static Result Validate(string email, string password)
    {
        if (!IsValidEmail(email) || !IsValidPassword(password))
            return Result.Failure("Invalid data. Please try again.");

        return Result.Success();
    }

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

    private static bool IsValidPassword(string password)
    {
        return password.Length is < 30 and > 6;
    }

    public void AddAccount(Account account)
    {
        Account = account;
    }
}
