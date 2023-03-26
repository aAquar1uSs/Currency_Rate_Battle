namespace CurrencyRateBattleServer.Domain.Entities.Errors;

public record CommonError(string ErrorCode, string Message) : Error(ErrorCode, Message);

