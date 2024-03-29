﻿using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class Currency
{
    public CurrencyName CurrencyName { get; private set; }

    public CurrencySymbol? CurrencySymbol { get; private set; }

    public Amount Rate { get; private set; }

    public string? Description { get; private set; }

    public DateTime UpdateDate { get; private set; }

    private Currency(CurrencyName currencyName,
        CurrencySymbol? currencySymbol,
        Amount rate,
        string? description,
        DateTime updateDate)
    {
        CurrencyName = currencyName;
        CurrencySymbol = currencySymbol;
        Rate = rate;
        Description = description;
        UpdateDate = updateDate;
    }

    private Currency(CurrencyName currencyCode,
        Amount rate, DateTime updateDate)
    {
        CurrencyName = currencyCode;
        Rate = rate;
        UpdateDate = updateDate;
    }

    public static Result<Currency> TryCreate(string currencyName, string currencySymbol,
        decimal amount, string description, DateTime updateDate)
    {
        var currencyNameResult = CurrencyName.TryCreate(currencyName);
        if (currencyNameResult.IsFailure)
            return Result.Failure<Currency>(currencyNameResult.Error);

        var currencyCodeResult = CurrencySymbol.TryCreate(currencySymbol);
        if (currencyCodeResult.IsFailure)
            return Result.Failure<Currency>(currencyCodeResult.Error);

        var amountResult = Amount.TryCreate(amount);
        if (amountResult.IsFailure)
            return Result.Failure<Currency>(amountResult.Error);

        return new Currency(currencyNameResult.Value,
            currencyCodeResult.Value, amountResult.Value, description, updateDate);
    }

    public static Currency Create(string currencyName, string currencySymbol,
        decimal amount, DateTime updateDate, string? description = null)
    {
        var currencyNameDomain = CurrencyName.Create(currencyName);

        var currencyCodeDomain = CurrencySymbol.Create(currencySymbol);

        var amountDomain = Amount.Create(amount);

        return new Currency(currencyNameDomain,
            currencyCodeDomain, amountDomain, description, updateDate);
    }

    public Result TryUpdateCurrency(decimal rate, string? date)
    {
        var rateResult = Amount.TryCreate(rate);
        if (rateResult.IsFailure)
            return Result.Failure(rateResult.Error);
        
        if (date is null || !DateTime.TryParse(date, out var dateTime))
            return Result.Failure("Can not parse the currency updated date");

        Rate = rateResult.Value;
        UpdateDate = dateTime;
        
        return Result.Success();
    } 
}
