using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;

    private readonly IAccountHistoryService _accountHistoryService;

    private readonly IRoomService _roomService;

    private readonly CurrencyRateBattleContext _dbContext;

    public PaymentService(ILogger<PaymentService> logger, CurrencyRateBattleContext dbContext,
        IAccountHistoryService accountHistoryService, IRoomService roomService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _accountHistoryService = accountHistoryService ?? throw new ArgumentNullException(nameof(accountHistoryService));
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
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

        var roomDal = await _roomService.FindAsync(roomId);
        
        accountHistory.AddRoom(roomDal.ToDomain());
        
        await _accountHistoryService.CreateAsync(accountHistory);
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
