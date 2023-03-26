using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class CurrencySymbol
{
    public string Value { get; }

    private CurrencySymbol(string value)
    {
        Value = value;
    }

    public static CurrencySymbol Create(string value) => new CurrencySymbol(value);

    public static Result<CurrencySymbol> TryCreate(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Failure<CurrencySymbol>("Currency symbols can not be null or empty");
        
        if (value.Length != 3)
            return Result.Failure<CurrencySymbol>("CurrencyCode can not be less or more than 3 symbols");

        return new CurrencySymbol(value);
    }

    public override string ToString()
    {
        return Value;
    }
}
