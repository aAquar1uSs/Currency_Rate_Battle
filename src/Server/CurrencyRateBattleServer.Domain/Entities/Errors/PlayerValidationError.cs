
namespace CurrencyRateBattleServer.Domain.Entities.Errors;

public record PlayerValidationError(string ErrorCode, string Message) : Error(ErrorCode, Message)
{
    public static PlayerValidationError AccountNotFound = new("account_not_found", "Account not found");
    public static PlayerValidationError UserNotFound = new("user_not_found", "User with such parameters doesn't exist");
    public static PlayerValidationError UserAlreadyExist = new("user_already_exist", "User with such email already exist");
}

