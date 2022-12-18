using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class Currency
{
    public CurrencyId Id { get; private set; }

    public CurrencyName CurrencyName { get; private set; }

    public CurrencyCode CurrencyCode { get; private set; }
    
    public Amount Rate { get; private set; }

    public string Description { get; private set; }

    private Currency(CurrencyId id,
        CurrencyName currencyName,
        CurrencyCode currencyCode,
        Amount rate,
        string description)
    {
        Id = id;
        CurrencyName = currencyName;
        CurrencyCode = currencyCode;
        Rate = rate;
        Description = description;
    }

    public static Result<Currency> TryCreate(Guid id, string currencyName, string currencyCode,
        decimal amount, string description)
    {
        var oneId = CurrencyId.TryCreate(id);
        if (oneId.IsFailure)
            return Result.Failure<Currency>(oneId.Error);

        var currencyNameResult = CurrencyName.TryCreate(currencyName);
        if (currencyNameResult.IsFailure)
            return Result.Failure<Currency>(currencyNameResult.Error);

        var currencyCodeResult = CurrencyCode.TryCreate(currencyCode);
        if (currencyCodeResult.IsFailure)
            return Result.Failure<Currency>(currencyCodeResult.Error);

        var amountResult = Amount.TryCreate(amount);
        if (amountResult.IsFailure)
            return Result.Failure<Currency>(amountResult.Error);

        return new Currency(oneId.Value, currencyNameResult.Value,
            currencyCodeResult.Value, amountResult.Value, description);
    }

    public static Currency Create(Guid id, string currencyName, string currencyCode,
        decimal amount, string description)
    {
        var oneId = CurrencyId.Create(id);

        var currencyNameDomain = CurrencyName.Create(currencyName);

        var currencyCodeDomain = CurrencyCode.Create(currencyCode);

        var amountDomain = Amount.Create(amount);
        
        return new Currency(oneId, currencyNameDomain,
            currencyCodeDomain, amountDomain, description);
    }

}
