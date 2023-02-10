using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class CurrencyState
{
    public CustomId Id { get; }

    public DateTime Date { get; set; }

    public Amount CurrencyExchangeRate { get; }

    public RoomId RoomId { get; }
    
    public CurrencyCode CurrencyCode { get; set; }
    
    public CurrencyName CurrencyName { get; set; }

    private CurrencyState(CustomId id,
        DateTime date,
        Amount currencyExchangeRate,
        RoomId roomId,
        CurrencyCode currencyCode,
        CurrencyName currencyName)
    {
        Id = id;
        Date = date;
        CurrencyExchangeRate = currencyExchangeRate;
        RoomId = roomId;
        CurrencyCode = currencyCode;
        CurrencyName = currencyName;
    }

    public static Result<CurrencyState> TryCreate(Guid id,
        DateTime date,
        decimal currencyExchange,
        Guid roomId,
        string currencyCode,
        string currencyName)
    {
        var oneIdResult = CustomId.TryCreate(id);
        if (oneIdResult.IsFailure)
            return Result.Failure<CurrencyState>(oneIdResult.Error);

        var currencyExchangeRateResult = Amount.TryCreate(currencyExchange);
        if (currencyExchangeRateResult.IsFailure)
            return Result.Failure<CurrencyState>(currencyExchangeRateResult.Error);

        var roomOneIdResult = RoomId.TryCreate(roomId);
        if (roomOneIdResult.IsFailure)
            return Result.Failure<CurrencyState>(roomOneIdResult.Error);

        var currencyCodeResult = CurrencyCode.TryCreate(currencyCode);
        if (currencyCodeResult.IsFailure)
            return Result.Failure<CurrencyState>(currencyCodeResult.Error);

        var currencyNameResult = CurrencyName.TryCreate(currencyName);
        if (currencyNameResult.IsFailure)    
            return Result.Failure<CurrencyState>(currencyCodeResult.Error);
        
        return new CurrencyState(oneIdResult.Value,
            date,
            currencyExchangeRateResult.Value,
            roomOneIdResult.Value,
            currencyCodeResult.Value,
            currencyNameResult.Value);
    }
    
    public static CurrencyState Create(Guid id,
        DateTime date,
        decimal currencyExchange,
        Guid roomId,
        string currencyCode,
        string currencyName)
    {
        var oneId = CustomId.Create(id);

        var currencyExchangeRate = Amount.Create(currencyExchange);

        var roomOneId = RoomId.Create(roomId);

        var currencyCodeDomain = CurrencyCode.Create(currencyCode);
        var currencyNameDomain = CurrencyName.Create(currencyName);
        return new CurrencyState(oneId,
            date,
            currencyExchangeRate,
            roomOneId,
            currencyCodeDomain,
            currencyNameDomain);
    }
}
