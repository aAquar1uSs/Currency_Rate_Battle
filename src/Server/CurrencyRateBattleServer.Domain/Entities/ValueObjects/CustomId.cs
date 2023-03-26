using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class CustomId
{
    public Guid Id { get; }
    
    public string Value { get; }

    protected CustomId(Guid id)
    {
        Id = id;
        Value = id.ToString();
    }

    public static Result<CustomId> TryCreate(Guid id)
    {
        if (id == Guid.Empty)
            Result.Failure<CustomId>("Id csn not be empty");

        return new CustomId(id);
    }

    public static CustomId Create(Guid id) => new(id);
    
    public static CustomId GenerateId() => new CustomId(Guid.NewGuid());
}
