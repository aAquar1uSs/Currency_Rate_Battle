namespace AccountManager.Domain.Errors;

public record UserValidationError : Error
{
    public UserValidationError(string type, string errorCode, string message)
        : base(type, errorCode, message)
    {
    }
}
