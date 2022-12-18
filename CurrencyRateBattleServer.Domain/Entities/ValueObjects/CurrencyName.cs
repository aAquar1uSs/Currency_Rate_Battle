using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class CurrencyName
{
    public string Value { get; }

    private CurrencyName(string value)
    {
        Value = value;
    }

    public static CurrencyName Create(string value) => new(value);

    public static Result<CurrencyName> TryCreate(string value)
    {
        if (string.IsNullOrEmpty(value))
            return Result.Failure<CurrencyName>("Currency symbols can not be null or empty");

        return new CurrencyName(value);
    }
}
