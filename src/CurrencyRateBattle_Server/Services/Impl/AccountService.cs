namespace CurrencyRateBattle_Server.Services.Impl;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;

    public AccountService(ILogger<AccountService> logger)
    {
        _logger = logger;
    }
}
