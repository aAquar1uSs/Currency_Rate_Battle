﻿using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class RoomId : CustomId
{
    protected RoomId(Guid id) : base(id)
    {
    }
    
    public static Result<RoomId> TryCreate(Guid? id)
    {
        if (id == Guid.Empty || id is null)
            Result.Failure<CustomId>("Room id can not be empty");

        return new RoomId(id.Value);
    }
    
    public static RoomId Create(Guid id) => new(id);
    
    public static RoomId GenerateId() => new(Guid.NewGuid());
}
