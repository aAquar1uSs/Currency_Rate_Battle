using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class AccountHistory
{
    public AccountHistoryId Id { get; private set; }

    public DateTime Date { get; private set; }

    public Amount Amount { get; private set; }

    public bool IsCredit { get; private set; }

    public RoomId? RoomId { get; private set; }

    public AccountId AccountId { get; private set; }


    private AccountHistory(AccountHistoryId id,
        bool isCredit,
        DateTime date,
        Amount amount,
        AccountId accountId,
        RoomId? roomId)
    {
        Id = id;
        IsCredit = isCredit;
        Date = date;
        Amount = amount;
        AccountId = accountId;
        RoomId = roomId;
    }
    
    public static AccountHistory Create(Guid accHistoryId ,Guid accId, DateTime date, decimal value,
        bool isCredit = false, Guid? roomId = null)
    {
        var accountHistoryId = AccountHistoryId.Create(accHistoryId);
        
        var accOneId = AccountId.Create(accId);

        var amountDomain = Amount.Create(value);

        if (roomId is null)
            return new AccountHistory(accountHistoryId, isCredit, date, amountDomain, accOneId, null);

        var roomOneId = RoomId.Create((Guid)roomId);

        return new AccountHistory(accountHistoryId, isCredit, date, amountDomain, accOneId, roomOneId);
    }

    public static Result<AccountHistory> TryCreate(Guid accHistoryId, Guid accId, DateTime date, decimal value,
        bool isCredit = false, Guid? roomId = null)
    {
        var accHistoryIdResult = AccountHistoryId.TryCreate(accHistoryId);
        if (accHistoryIdResult.IsFailure)
            return Result.Failure<AccountHistory>(accHistoryIdResult.Error);
        
        var accIdResult = AccountId.TryCreate(accId);
        if (accIdResult.IsFailure)
        {
            return Result.Failure<AccountHistory>(accIdResult.Error);
        }

        var amountResult = Amount.TryCreate(value);
        if (amountResult.IsFailure)
        {
            return Result.Failure<AccountHistory>(amountResult.Error);
        }

        if (roomId is null)
        {
            return new AccountHistory(accHistoryIdResult.Value, isCredit, date, amountResult.Value, accIdResult.Value, null);
        }

        var roomIdResult = RoomId.TryCreate((Guid)roomId);
        if (roomIdResult.IsFailure)
        {
            return Result.Failure<AccountHistory>(roomIdResult.Error);
        }
        
        return new AccountHistory(accHistoryIdResult.Value, isCredit, date, amountResult.Value, accIdResult.Value, roomIdResult.Value);
    }
}
