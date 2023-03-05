using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class AccountHistoriesConverter
{
    public static AccountHistoryDto[] ToDto(this AccountHistory[] accountHistories)
    {
        return accountHistories.Select(ToDto).ToArray();
    }

    private static AccountHistoryDto ToDto(this AccountHistory accountHistory)
    {
        return new AccountHistoryDto
        {
            AccountHistoryId = accountHistory.Id.Id,
            Amount = accountHistory.Amount.Value,
            Date = accountHistory.Date,
            IsCredit = accountHistory.IsCredit,
            RoomId = accountHistory.RoomId?.Id
        };
    }

    public static CreateHistoryCommand ToCreateCommand(this AccountHistoryDto dto, string userEmail)
    {
        return new CreateHistoryCommand(userEmail, dto.RoomId, dto.Date, dto.Amount, dto.IsCredit);
    }
}
