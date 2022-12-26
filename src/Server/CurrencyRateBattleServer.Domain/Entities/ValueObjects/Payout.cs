using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class Payout : Amount
{
    protected Payout(decimal value) : base(value)
    {
    }

    public static Result<Payout> TryCreate(decimal value)
    {
        return value < 0 ? Result.Failure<Payout>($"Failed created Payout.{value} can not be less than 0") : new Payout(value);
    }

    public static Payout Create(decimal value) => new(value);
}
