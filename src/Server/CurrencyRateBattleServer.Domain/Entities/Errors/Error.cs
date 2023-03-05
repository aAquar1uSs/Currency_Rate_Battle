namespace CurrencyRateBattleServer.Domain.Entities.Errors;

public abstract record Error
{
    protected Error(string errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }
    
    protected string ErrorCode { get; }
    
    protected string Message { get; }
}
