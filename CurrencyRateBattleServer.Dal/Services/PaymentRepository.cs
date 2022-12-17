using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class PaymentRepository : IPaymentRepository
{
    private readonly ILogger<PaymentRepository> _logger;

    private readonly IAccountHistoryRepository _accountHistoryRepository;

    private readonly IRoomRepository _roomRepository;

    private readonly CurrencyRateBattleContext _dbContext;

    public PaymentRepository(ILogger<PaymentRepository> logger, CurrencyRateBattleContext dbContext,
        IAccountHistoryRepository accountHistoryRepository, IRoomRepository roomRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _accountHistoryRepository = accountHistoryRepository ?? throw new ArgumentNullException(nameof(accountHistoryRepository));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
    }

    public async Task ApportionCashByRateAsync(Guid roomId, Guid accountId, decimal? payout)
    {
        _logger.LogInformation($"{nameof(ApportionCashByRateAsync)} was caused.");

        var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);

        if (account is null || payout is null)
            return;

        account.Amount += (decimal)payout;

        _ = await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Successful payment");
        //ToDo Move this to handler
        var accountHistory = AccountHistory.Create(accountId, DateTime.UtcNow, (decimal)payout, true);

        var roomDal = await _roomRepository.GetRoomByIdAsync(roomId);
        
        accountHistory.AddRoom(roomDal.ToDomain());
        
        await _accountHistoryRepository.CreateAsync(accountHistory);
    }

    public async Task<bool> WritingOffMoneyAsync(Account account, decimal? amount)
    {
        _logger.LogInformation($"{nameof(WritingOffMoneyAsync)} was caused.");

        if (amount is null)
            return false;

        if (account.Amount == 0 || amount > account.Amount)
            return false;

        account.Amount -= (decimal)amount;

        _ = await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Successful withdrawal");

        return true;
    }
}
