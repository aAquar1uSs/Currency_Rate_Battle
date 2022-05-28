using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IAccountHistoryService _accountHistoryService;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public PaymentService(ILogger<PaymentService> logger,
        IServiceScopeFactory scopeFactory,
        IAccountHistoryService accountHistoryService)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _accountHistoryService = accountHistoryService;
    }

    public async Task ApportionCashByRateAsync(Guid roomId, Guid accountId, decimal? payout)
    {
        _logger.LogInformation($"{nameof(ApportionCashByRateAsync)} was caused.");
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await _semaphoreSlim.WaitAsync();
        try
        {
            var account = await db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);

            if (account is null || payout is null)
                return;

            account.Amount += (decimal)payout;

            _ = await db.SaveChangesAsync();
            _logger.LogInformation("Successful payment");
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        await _accountHistoryService.CreateHistoryByValuesAsync(roomId, accountId,
            DateTime.UtcNow, (decimal)payout, true);
    }

    public async Task<bool> WritingOffMoneyAsync(Guid accountId, decimal? amount)
    {
        _logger.LogInformation($"{nameof(WritingOffMoneyAsync)} was caused.");

        if (amount is null)
            return false;

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        await _semaphoreSlim.WaitAsync();
        try
        {
            var account = await db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);

            if (account is null)
                return false;

            if (account.Amount == 0 || amount  > account.Amount)
                return false;

            account.Amount -= (decimal)amount;

            _ = await db.SaveChangesAsync();
            _logger.LogInformation("Successful withdrawal");
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return true;
    }
}
