namespace CurrencyRateBattleServer.Domain.Entities.Errors;

public record RoomValidationError(string ErrorCode, string Message) : Error(ErrorCode, Message)
{
    public static RoomValidationError NotFound = new("room_not_found", "Room not found.");
}
