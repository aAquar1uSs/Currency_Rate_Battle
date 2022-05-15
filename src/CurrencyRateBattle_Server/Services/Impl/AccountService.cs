using CurrencyRateBattle_Server.Contexts;
using CurrencyRateBattle_Server.Dto;
using CurrencyRateBattle_Server.Managers;
using CurrencyRateBattle_Server.Models;
using CurrencyRateBattle_Server.Tools;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattle_Server.Services.Impl;

public class AccountService : IAccountService
{
    private readonly ILogger<IAccountService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IJwtManager _jwtManager;

    private readonly IEncoder _encoder;

    private const decimal AccountStartBalance = 10000;

    public AccountService(ILogger<AccountService> logger,
        IServiceScopeFactory scopeFactory,
        IJwtManager jwtManager,
        IEncoder encoder)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _jwtManager = jwtManager;
        _encoder = encoder;
    }

    public async Task<Tokens?> LoginAsync(UserDto userData)
    {
        var user = new User
        {
            Email = userData.Email,
            Password = _encoder.Encrypt(userData.Password)
        };

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        if (db.Users is null)
            return null;

        if (!await db.Users.AnyAsync(x => x.Email == user.Email && x.Password == user.Password))
        {
            return null;
        }

        return _jwtManager.Authenticate(user);
    }

    public async Task<Tokens?> RegistrationAsync(UserDto userData)
    {
        var user = new User
        {
            Email = userData.Email,
            Password = _encoder.Encrypt(userData.Password),
            Bill = new Account()
            {
                Amount = AccountStartBalance
            }
        };

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        if (db.Users is null)
            return null;

        if (await db.Users.AnyAsync(user => user.Email == userData.Email))
            return null;

        _ = await db.Users.AddAsync(user);
        _ = await db.SaveChangesAsync();

        return _jwtManager.Authenticate(user);
    }
}
