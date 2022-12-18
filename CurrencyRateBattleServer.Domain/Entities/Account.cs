using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public sealed class Account
{
    public AccountId Id { get; private set; }

    public Amount Amount { get; private set; }

    public UserId UserId { get; private set; }

    private Account(AccountId id,
        Amount amount,
        UserId userId)
    {
        Id = id;
        Amount = amount;
        UserId = userId;
    }

    public static Result<Account> TryCreate(Guid id, decimal amount, Guid userId)
    {
        var amountResult = Amount.TryCreate(amount);
        if (amountResult.IsFailure)
            return Result.Failure<Account>(amountResult.Error);

        var accIdResult = AccountId.TryCreate(id);
        if(amountResult.IsFailure)
            return Result.Failure<Account>(amountResult.Error);

        var userIdResult = UserId.TryCreate(userId);
        if(amountResult.IsFailure)
            return Result.Failure<Account>(amountResult.Error);

        return new Account(accIdResult.Value, amountResult.Value, userIdResult.Value);
    }

    public static Account Create(Guid id, decimal amount, Guid userId)
    {
        var accOneId = AccountId.Create(id);
        var amountDomain = Amount.Create(amount);
        var userOneId = UserId.Create(userId);

        return new Account (accOneId, amountDomain, userOneId);
    }

    public static Result<Account> TryCreateNewAccount(Guid id, Guid userId)
    {
        var accountIdResult = AccountId.TryCreate(id);
        if (accountIdResult.IsFailure)
            return Result.Failure<Account>(accountIdResult.Error);

        var userIdResult = UserId.TryCreate(userId);
        if (userIdResult.IsFailure)
            return Result.Failure<Account>(userIdResult.Error);

        return new Account(accountIdResult.Value, Amount.Create(0), userIdResult.Value);
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

        Amount.ApportionCash(money.Value);
        
        return Result.Success();
    }
}
