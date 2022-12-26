using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class UserId : OneId
{
    protected UserId(Guid id) : base(id)
    {
        
    }
    
    public static Result<UserId> TryCreate(Guid? id)
    {
        if (id == Guid.Empty || id is null)
            Result.Failure<OneId>("User Id can not be empty or null");

        return new UserId(id.Value);
    }
    
    public static UserId Create(Guid id) => new(id);
    
    public static UserId GenerateId() => new(Guid.NewGuid());
}
