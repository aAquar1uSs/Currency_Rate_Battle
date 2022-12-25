using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class CurrencyName
{
    public string Value { get; }

    private CurrencyName(string value)
    {
        Value = value;
    }

    public static CurrencyName Create(string value) => new CurrencyName(value);

    public static Result<CurrencyName> TryCreate(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Failure<CurrencyName>("Currency symbols can not be null or empty");
        
        if (value.Length != 3)
            return Result.Failure<CurrencyName>("CurrencyCode can not be less or more than 3 symbols");

        return new CurrencyName(value);
    }

    public override string ToString()
    {
        return Value;
    }
}
