using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class CurrencyId : OneId
{
    protected CurrencyId(Guid id) : base(id)
    {
    }
    
    public static Result<CurrencyId> TryCreate(Guid id)
    {
        if (id == Guid.Empty)
            Result.Failure<OneId>("User Id can not be empty");

        return new CurrencyId(id);
    }
    
    public static CurrencyId Create(Guid id) => new CurrencyId(id);
    
    public static CurrencyId GenerateAccountId() => new CurrencyId(Guid.NewGuid());
}
