namespace CurrencyRateBattle_Server.Services.Impl;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public AccountService(ILogger<AccountService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
}
