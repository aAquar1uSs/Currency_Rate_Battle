using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class OneId
{
    public Guid Id { get; }
    
    public string Value { get; }

    protected OneId(Guid id)
    {
        Id = id;
        Value = id.ToString();
    }

    public static Result<OneId> TryCreate(Guid id)
    {
        if (id == Guid.Empty)
            Result.Failure<OneId>("Id csn not be empty");

        return new OneId(id);
    }

    public static OneId Create(Guid id) => new(id);
    
    public static OneId GenerateId() => new OneId(Guid.NewGuid());
}
