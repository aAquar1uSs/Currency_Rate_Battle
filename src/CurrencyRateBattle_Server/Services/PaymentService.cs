using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public PaymentService(ILogger<PaymentService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task ApportionCashByRateAsync(Guid accountId, decimal amount)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var account = await db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);

        if (account is null)
            return;

        account.Amount += amount;

        _ = await db.SaveChangesAsync();
    }
}
