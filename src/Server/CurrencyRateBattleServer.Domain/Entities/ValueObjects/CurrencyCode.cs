using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class CurrencyCode
{
    public string Value { get; }

    private CurrencyCode(string value)
    {
        Value = value;
    }

    public static CurrencyCode Create(string value) => new CurrencyCode(value);

    public static Result<CurrencyCode> TryCreate(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Failure<CurrencyCode>("Currency symbols can not be null or empty");
        
        if (value.Length != 3)
            return Result.Failure<CurrencyCode>("CurrencyCode can not be less or more than 3 symbols");

        return new CurrencyCode(value);
    }

    public override string ToString()
    {
        return Value;
    }
}
