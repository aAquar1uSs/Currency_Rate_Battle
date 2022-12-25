using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class RoomId : OneId
{
    protected RoomId(Guid id) : base(id)
    {
    }
    
    public static Result<RoomId> TryCreate(Guid id)
    {
        if (id == Guid.Empty)
            Result.Failure<OneId>("Room id can not be empty");

        return new RoomId(id);
    }
    
    public static RoomId Create(Guid id) => new(id);
    
    public static RoomId GenerateId() => new(Guid.NewGuid());
}
