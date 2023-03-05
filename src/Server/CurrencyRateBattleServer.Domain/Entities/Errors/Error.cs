namespace CurrencyRateBattleServer.Domain.Entities.Errors;

public abstract record Error
{
    protected Error(string errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }
    
    public string ErrorCode { get; }
    
    public string Message { get; }
}
