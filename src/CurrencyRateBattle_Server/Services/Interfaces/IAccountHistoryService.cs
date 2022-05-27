using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Models;

namespace CurrencyRateBattleServer.Services.Interfaces;

public interface IAccountHistoryService
{
    Task<List<AccountHistory>> GetAccountHistoryByAccountId(Guid? id);

    Task CreateHistoryAsync(Room? room, Account account, AccountHistoryDto accountHistoryDto);

    Task CreateHistoryByValuesAsync(Guid? roomId, Guid accountId, DateTime recordDate, decimal amount, bool isCredit);
}
