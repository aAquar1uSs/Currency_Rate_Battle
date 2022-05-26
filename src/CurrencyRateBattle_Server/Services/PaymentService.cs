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
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var account = await db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);

        if (account is null || payout is null)
            return;

        account.Amount += (decimal)payout;

        _ = await db.SaveChangesAsync();

        await _accountHistoryService.CreateHistoryByValuesAsync(roomId, accountId,
            DateTime.UtcNow, (decimal)payout, true);
    }

    public async Task<bool> WritingOffMoneyAsync(Guid accountId, decimal? amount)
    {
        if (amount is null)
            return false;

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var account = await db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);

        if (account is null)
            return false;

        if (account.Amount == 0 || amount  > account.Amount)
            return false;
        await _semaphoreSlim.WaitAsync();
        try
        {
            account.Amount -= (decimal)amount;

            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return true;
    }
}
