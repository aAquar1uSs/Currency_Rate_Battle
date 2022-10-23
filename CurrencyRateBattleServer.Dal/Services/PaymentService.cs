using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;

    private readonly IAccountHistoryService _accountHistoryService;

    private readonly CurrencyRateBattleContext _dbContext;

    public PaymentService(ILogger<PaymentService> logger,
        CurrencyRateBattleContext dbContext,
        IAccountHistoryService accountHistoryService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _accountHistoryService = accountHistoryService;
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

        await _accountHistoryService.CreateHistoryByValuesAsync(roomId, accountId,
            DateTime.UtcNow, (decimal)payout, true);
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
