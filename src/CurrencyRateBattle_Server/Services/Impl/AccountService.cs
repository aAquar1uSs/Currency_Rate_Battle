using CurrencyRateBattle_Server.Contexts;
using CurrencyRateBattle_Server.Managers;
using CurrencyRateBattle_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattle_Server.Services.Impl;

public class AccountService : IAccountService
{
    private readonly ILogger<IAccountService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IJwtManager _jwtManager;

    public AccountService(ILogger<IAccountService> logger,
        IServiceScopeFactory scopeFactory,
        IJwtManager jwtManager)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _jwtManager = jwtManager;
    }

    public async Task<Tokens?> LoginAsync(User userData)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        if (db.Users is null)
            return null;

        if (!await db.Users.AnyAsync(x => x.Email == userData.Email && x.Password == userData.Password))
        {
            return null;
        }

        return _jwtManager.Authenticate(userData);
    }
}
