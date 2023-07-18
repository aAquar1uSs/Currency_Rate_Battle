namespace AccountManager.Domain.Errors;

public abstract record Error
{
    public Error(string type, string errorCode, string message)
    {
        Type = type;
        ErrorCode = errorCode;
        Message = message;
    }
    
    public string Type { get; }
    
    public string ErrorCode { get; set; }
    
    public string Message { get; set; }
}
