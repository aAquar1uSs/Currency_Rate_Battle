using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class Room
{
    public RoomId Id { get; }

    public DateTime EndDate { get; }

    public bool IsClosed { get; }
    
    public int CountRates { get; set; }
    
    public CurrencyName CurrencyName { get; set; }

    private Room(RoomId id,
        DateTime date,
        bool isClosed,
        int countRates,
        CurrencyName currencyName)
    {
        Id = id;
        EndDate = date;
        IsClosed = isClosed;
        CountRates = countRates;
        CurrencyName = currencyName;
    }

    public Result<Room> TryCreate(Guid id, DateTime date, bool isClosed, int countRates, string currencyName)
    {
        var oneIdResult = RoomId.TryCreate(id);
        if (oneIdResult.IsFailure)
            return Result.Failure<Room>(oneIdResult.Error);

        var currencyNameResult = CurrencyName.TryCreate(currencyName);
        if (currencyNameResult.IsFailure)
            return Result.Failure<Room>(currencyNameResult.Error);

        if (countRates < 0)
            return Result.Failure<Room>("Count of rates can not be null");

        return new Room(oneIdResult.Value, date, isClosed, countRates, currencyNameResult.Value);
    }

    public static Room Create(Guid id, DateTime date, bool isClosed, int countRates, string currencyName)
    {
        var oneId = RoomId.Create(id);

        var currencyNameDomain = CurrencyName.Create(currencyName);
        
        return new Room(oneId, date, isClosed, countRates, currencyNameDomain);
    }

    public Result IncrementCountRates()
    {
        if (EndDate <= DateTime.UtcNow)
            return Result.Failure("Room is closed.");
        
        CountRates++;
        
        return Result.Success();
    }
}
