using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class Currency
{
    public CurrencyName? CurrencyName { get; private set; }

    public CurrencyCode CurrencyCode { get; private set; }
    
    public Amount Rate { get; private set; }

    public string? Description { get; private set; }

    private Currency(CurrencyName currencyName,
        CurrencyCode currencyCode,
        Amount rate,
        string? description)
    {
        CurrencyName = currencyName;
        CurrencyCode = currencyCode;
        Rate = rate;
        Description = description;
    }
    
    private Currency(CurrencyCode currencyCode,
        Amount rate)
    {
        CurrencyCode = currencyCode;
        Rate = rate;
    }

    public static Result<Currency> TryCreate(string currencyName, string currencyCode,
        decimal amount, string description)
    {
        var currencyNameResult = CurrencyName.TryCreate(currencyName);
        if (currencyNameResult.IsFailure)
            return Result.Failure<Currency>(currencyNameResult.Error);

        var currencyCodeResult = CurrencyCode.TryCreate(currencyCode);
        if (currencyCodeResult.IsFailure)
            return Result.Failure<Currency>(currencyCodeResult.Error);

        var amountResult = Amount.TryCreate(amount);
        if (amountResult.IsFailure)
            return Result.Failure<Currency>(amountResult.Error);

        return new Currency(currencyNameResult.Value,
            currencyCodeResult.Value, amountResult.Value, description);
    }

    public static Currency Create(string currencyName, string currencyCode,
        decimal amount, string? description)
    {
        var currencyNameDomain = CurrencyName.Create(currencyName);

        var currencyCodeDomain = CurrencyCode.Create(currencyCode);

        var amountDomain = Amount.Create(amount);
        
        return new Currency(currencyNameDomain,
            currencyCodeDomain, amountDomain, description);
    }
    
    public static Currency Create(string currencyCode,
        decimal amount)
    {
        var currencyCodeDomain = CurrencyCode.Create(currencyCode);

        var amountDomain = Amount.Create(amount);
        
        return new Currency(currencyCodeDomain, amountDomain);
    }

}
