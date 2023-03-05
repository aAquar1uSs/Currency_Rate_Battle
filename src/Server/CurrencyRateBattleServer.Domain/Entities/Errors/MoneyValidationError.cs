namespace CurrencyRateBattleServer.Domain.Entities.Errors;

public record MoneyValidationError(string ErrorCode, string Message) : Error(ErrorCode, Message);
