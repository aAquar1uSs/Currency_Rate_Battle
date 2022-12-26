using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class AccountHistoryId : OneId
{
    protected AccountHistoryId(Guid id) : base(id)
    {
    }
    
    public static Result<AccountHistoryId> TryCreate(Guid id)
    {
        if (id == Guid.Empty)
            Result.Failure<OneId>("account history id can not be empty");

        return new AccountHistoryId(id);
    }
    
    public static AccountHistoryId Create(Guid id) => new(id);
    
    public static AccountHistoryId GenerateId() => new(Guid.NewGuid());
    
}
