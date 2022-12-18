using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class Amount
{
    public decimal Value { get; private set; }

    protected Amount(decimal value)
    {
        Value = value;
    }

    public static Result<Amount> TryCreate(decimal value)
    {
        return value < 0 ? Result.Failure<Amount>($"{value} can not be less than 0") : new Amount(value);
    }

    public static Amount Create(decimal value) => new(value);

    public void WithdrawalMoney(decimal money)
    {
        Value -= money;
    }
}
