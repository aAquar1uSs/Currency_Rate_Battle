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

    private readonly IAccountHistoryService _accountHistoryService;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    private readonly decimal _accountStartBalance;

    public AccountService(IOptions<WebServerOptions> options,
        ILogger<AccountService> logger,
        IServiceScopeFactory scopeFactory,
        IJwtManager jwtManager,
        IEncoder encoder,
        IAccountHistoryService accountHistoryService)
    {
        _options = options.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;
        _jwtManager = jwtManager;
        _encoder = encoder;
        _accountHistoryService = accountHistoryService;
        _accountStartBalance = _options.RegistrationReward;
    }

    public async Task<Tokens?> GetUserAsync(UserDto userData)
    {
        _logger.LogDebug($"{nameof(GetUserAsync)} was caused.");

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var user = await db.Users
            .FirstOrDefaultAsync(x => x.Email == userData.Email && x.Password == _encoder.Encrypt(userData.Password));

        return user is null ? null! : _jwtManager.Authenticate(user);
    }

    public async Task<Tokens?> CreateUserAsync(UserDto userData)
    {
        var user = new User
        {
            Email = userData.Email,
            Password = _encoder.Encrypt(userData.Password),
            Account = new Account {Amount = _accountStartBalance}
        };

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        if (await db.Users.AnyAsync(u => u.Email == userData.Email))
            throw new GeneralException("Email '" + user.Email + "' is already taken");

        await _semaphoreSlim.WaitAsync();
        try
        {
            _ = await db.Users.AddAsync(user);
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        _logger.LogInformation("New user added to the database");

        await _accountHistoryService.CreateHistoryByValuesAsync(null, user.Account.Id, DateTime.UtcNow,
            _accountStartBalance, true);

        _ = await db.SaveChangesAsync();

        return _jwtManager.Authenticate(user);
    }

    public async Task<AccountInfoDto?> GetAccountInfoByUserIdAsync(Guid id)
    {
        _logger.LogDebug($"{nameof(GetAccountInfoByUserIdAsync)} was caused.");

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var account = await db.Accounts.FirstOrDefaultAsync(a => a.UserId == id);

        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null || account is null)
            return null;

        var accountInfoDto = new AccountInfoDto {Email = user.Email, Amount = account.Amount};
        return accountInfoDto;
    }

    public async Task<Account?> GetAccountByUserIdAsync(Guid? userId)
    {
        _logger.LogDebug($"{nameof(GetAccountInfoByUserIdAsync)} was caused.");

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var account = await db.Accounts
                .FirstOrDefaultAsync(acc => acc.UserId == userId);

        return account;
    }

    public Guid? GetGuidFromRequest(HttpContext context)
    {
        _logger.LogDebug($"{nameof(GetGuidFromRequest)} was caused.");
        var user = context.User;

        if (user.HasClaim(c => c.Type == "UserId"))
        {
            var id = Guid.Parse(user.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value);
            return id;
        }

        return null!;
    }
}
