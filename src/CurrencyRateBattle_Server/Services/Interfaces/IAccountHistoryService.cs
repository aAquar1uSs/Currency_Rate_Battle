using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IAccountHistoryService
{
    Task<List<AccountHistory>> GetAccountHistoryByAccountId(Guid? id);

    Task CreateHistoryAsync(Room room, Account account, AccountHistoryDto history);
}
