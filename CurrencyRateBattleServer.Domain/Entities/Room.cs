using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class Room
{
    public RoomId Id { get; }

    public DateTime CreatedDate { get; }

    public bool IsClosed { get; }

    private Room(RoomId id,
        DateTime date,
        bool isClosed)
    {
        Id = id;
        CreatedDate = date;
        IsClosed = isClosed;
    }

    public Result<Room> TryCreate(Guid id, DateTime date, bool isClosed)
    {
        var oneIdResult = RoomId.TryCreate(id);
        if (oneIdResult.IsFailure)
            return Result.Failure<Room>(oneIdResult.Error);

        return new Room(oneIdResult.Value, date, isClosed);
    }

    public static Room Create(Guid id, DateTime date, bool isClosed)
    {
        var oneId = RoomId.Create(id);

        return new Room(oneId, date, isClosed);
    }
}
