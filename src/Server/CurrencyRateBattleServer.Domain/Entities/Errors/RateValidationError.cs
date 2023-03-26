namespace CurrencyRateBattleServer.Domain.Entities.Errors;

public record RateValidationError(string ErrorCode, string Message) : Error(ErrorCode, Message)
{
    
}
