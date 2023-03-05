
namespace CurrencyRateBattleServer.Domain.Entities.Errors;

//ToDo use this in handlers
public record PlayerValidationError(string ErrorCode, string Message) : Error(ErrorCode, Message)
{
    public static PlayerValidationError AccountNotFound = new("account_not_found", "Account not found");
}

