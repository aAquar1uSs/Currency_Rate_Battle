using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities.Errors;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class ErrorDtoConverter
{
    public static ErrorDto ToDto(this Error error)
    {
        return new() { ErrorCode = error.ErrorCode, Message = error.Message };
    }
}
