using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public sealed class Account
{
    public AccountId Id { get; private set; }

    public Amount Amount { get; private set; }

    public Email UserEmail { get; private set; }

    private Account(AccountId id,
        Amount amount,
        Email userEmail)
    {
        Id = id;
        Amount = amount;
        UserEmail = userEmail;
    }

    public static Result<Account> TryCreate(Guid id, decimal amount, string userEmail)
    {
        var amountResult = Amount.TryCreate(amount);
        if (amountResult.IsFailure)
            return Result.Failure<Account>(amountResult.Error);

        var accIdResult = AccountId.TryCreate(id);
        if(amountResult.IsFailure)
            return Result.Failure<Account>(amountResult.Error);

        var userIdResult = Email.TryCreate(userEmail);
        if(amountResult.IsFailure)
            return Result.Failure<Account>(amountResult.Error);

        return new Account(accIdResult.Value, amountResult.Value, userIdResult.Value);
    }

    public static Account Create(Guid id, decimal amount, string userEmail)
    {
        var accOneId = AccountId.Create(id);
        var amountDomain = Amount.Create(amount);
        var userOneId = Email.Create(userEmail);

        return new Account (accOneId, amountDomain, userOneId);
    }

    public static Account TryCreateNewAccount(AccountId id, Email userEmail, Amount startBalance)
    {
        var account = new Account(id, Amount.Create(0), userEmail);
        account.AddStartBalance(startBalance);
        return account;
    }

    public void AddStartBalance(Amount startBalance)
    {
        Amount = startBalance;
    }

    public Result WritingOffMoney(Amount money)
    {
        if (money is null)
            return Result.Failure<Account>("Payment processing error");

        if (Amount.Value == 0 || money.Value > Amount.Value)
            return Result.Failure<Account>("Payment processing error");

        Amount.WithdrawalMoney(money.Value);

        return Result.Success();
    }

    public Result ApportionCash(Amount money)
    {
        if (money is null)
            return Result.Failure<Account>("Payment processing error");

        Amount.ApportionMoney(money.Value);

        return Result.Success();
    }
}
