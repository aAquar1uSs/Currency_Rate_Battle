using AccountManager.Domain.Errors;
using AccountManager.Dto;

namespace AccountManager.Converters;

public static class ErrorConverter
{
    public static ErrorDto ToDto(this Error error)
    {
        return new ErrorDto { ErrorCode = error.ErrorCode, Message = error.Message };
    }
}
