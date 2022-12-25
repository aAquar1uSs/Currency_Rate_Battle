using CurrencyRateBattleServer.ApplicationServices.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;

public class GetAccountHistoryResponse
{
    public AccountHistoryDto[] AccountHistories { get; set; }
}
