using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class CurrencyState
{
    public OneId Id { get; }

    public DateTime Date { get; set; }

    public Amount CurrencyExchangeRate { get; }

    public RoomId RoomId { get; }

    public CurrencyId CurrencyId { get; }

    private CurrencyState(OneId id,
        DateTime date,
        Amount currencyExchangeRate,
        RoomId roomId,
        CurrencyId currencyId)
    {
        Id = id;
        Date = date;
        CurrencyExchangeRate = currencyExchangeRate;
        RoomId = roomId;
        CurrencyId = currencyId;
    }

    public static Result<CurrencyState> TryCreate(Guid id, DateTime date, decimal currencyExchange, Guid roomId,
        Guid currencyId)
    {
        var oneIdResult = OneId.TryCreate(id);
        if (oneIdResult.IsFailure)
            return Result.Failure<CurrencyState>(oneIdResult.Error);

        var currencyExchangeRateResult = Amount.TryCreate(currencyExchange);
        if (currencyExchangeRateResult.IsFailure)
            return Result.Failure<CurrencyState>(currencyExchangeRateResult.Error);

        var roomOneIdResult = RoomId.TryCreate(roomId);
        if (roomOneIdResult.IsFailure)
            return Result.Failure<CurrencyState>(roomOneIdResult.Error);

        var currencyOneIdResult = CurrencyId.TryCreate(currencyId);
        if (currencyExchangeRateResult.IsFailure)
            return Result.Failure<CurrencyState>(currencyExchangeRateResult.Error);

        return new CurrencyState(oneIdResult.Value, date, currencyExchangeRateResult.Value, roomOneIdResult.Value,
            currencyOneIdResult.Value);
    }
    
    public static CurrencyState Create(Guid id, DateTime date, decimal currencyExchange, Guid roomId,
        Guid currencyId)
    {
        var oneId = OneId.Create(id);

        var currencyExchangeRate = Amount.Create(currencyExchange);

        var roomOneId = RoomId.Create(roomId);

        var currencyOneId = CurrencyId.Create(currencyId);

        return new CurrencyState(oneId, date, currencyExchangeRate, roomOneId,
            currencyOneId);
    }
}
