using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Managers.Interfaces;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using CurrencyRateBattleServer.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CurrencyRateBattleServer.Services;

public class AccountService : IAccountService
{
    private readonly WebServerOptions _options;

    private readonly ILogger<IAccountService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IJwtManager _jwtManager;

    private readonly IEncoder _encoder;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    private readonly decimal _accountStartBalance;

    public AccountService(IOptions<WebServerOptions> options,
        ILogger<AccountService> logger,
        IServiceScopeFactory scopeFactory,
        IJwtManager jwtManager,
        IEncoder encoder)
    {
        _options = options.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;
        _jwtManager = jwtManager;
        _encoder = encoder;
        _accountStartBalance = _options.RegistrationReward;
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
                Amount = _accountStartBalance
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
