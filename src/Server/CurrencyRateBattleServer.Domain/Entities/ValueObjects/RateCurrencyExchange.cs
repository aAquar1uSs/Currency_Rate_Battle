using CSharpFunctionalExtensions;

namespace CurrencyRateBattleServer.Domain.Entities.ValueObjects;

public class RateCurrencyExchange : Amount
{
    protected RateCurrencyExchange(decimal value) : base(value)
    {
        
    }
    
    public static Result<RateCurrencyExchange> TryCreate(decimal value)
    {
        return value < 0 ? Result.Failure<RateCurrencyExchange>($"Failed created RateCurrencyExchange.{value} can not be less than 0") : new RateCurrencyExchange(value);
    }

    public static RateCurrencyExchange Create(decimal value) => new(value);
}
