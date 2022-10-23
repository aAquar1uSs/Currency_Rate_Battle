using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Dto;

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
            AccountHistoryId = accountHistory.Id,
            Amount = accountHistory.Amount,
            Date = accountHistory.Date,
            IsCredit = accountHistory.IsCredit,
            RoomId = accountHistory.RoomId
        };
    }

    public static AccountHistory ToDomain(this AccountHistoryDto accountHistoryDto)
    {
        return new AccountHistory
        {
            Amount = accountHistoryDto.Amount, Date = accountHistoryDto.Date, IsCredit = accountHistoryDto.IsCredit
        };
    }
}
