using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Managers.Interfaces;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using CurrencyRateBattleServer.Tools;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<IAccountService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IJwtManager _jwtManager;

    private readonly IEncoder _encoder;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

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

    public async Task<Tokens?> GetUserAsync(UserDto userData)
    {
        var user = new User
        {
            Email = userData.Email,
            Password = _encoder.Encrypt(userData.Password)
        };

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        if (!await db.Users.AnyAsync(x => x.Email == user.Email && x.Password == user.Password))
            return null;

        return _jwtManager.Authenticate(user);
    }

    public async Task<Tokens?> СreateUserAsync(UserDto userData)
    {
        var user = new User
        {
            Email = userData.Email,
            Password = _encoder.Encrypt(userData.Password),
            Account = new Account
            {
                Amount = AccountStartBalance
            }
        };

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        if (await db.Users.AnyAsync(u => u.Email == userData.Email))
            throw new CustomException("Email '" + user.Email + "' is already taken");

        await _semaphoreSlim.WaitAsync();
        try
        {
            _ = await db.Users.AddAsync(user);
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return _jwtManager.Authenticate(user);
    }
}
