using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.AccountHandlers.Registration;

public class RegistrationHandler : IRequestHandler<RegistrationCommand, Result<RegistrationResponse>>
{
    private readonly ILogger<RegistrationHandler> _logger;

    private readonly IAccountService _accountService;
    
    private readonly IAccountHistoryService _accountHistoryService;

    private readonly IUserService _userService;

    private readonly WebServerOptions _options;

    private readonly IJwtManager _jwtManager;

    public RegistrationHandler(ILogger<RegistrationHandler> logger, IAccountService accountService, 
        IOptions<WebServerOptions> options, IAccountHistoryService historyService, JwtManager jwtManager,
        IUserService userService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _accountHistoryService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
    }

    public async Task<Result<RegistrationResponse>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(RegistrationHandler)} was caused.");
        
        var userResult = User.Create(request.UserDto.Email, request.UserDto.Password);

        if (userResult.IsFailure)
        {
            _logger.LogWarning(userResult.Error);
            return Result.Failure<RegistrationResponse>(userResult.Error);   
        }

        var user = userResult.Value;
        var maybeUser = await _userService.FindAsync(user);

        if (maybeUser is not null)
            return Result.Failure<RegistrationResponse>("User with such email already exist");

        var account = Account.Create();
        account.AddStartBalance(_options.RegistrationReward);
        
        await _accountService.CreateAccountAsync(account);
        
        user.AddAccount(account);

        await _userService.CreateAsync(user);

        var accountHistory = AccountHistory.Create(account.Id, DateTime.UtcNow, account.Amount, true);
        await _accountHistoryService.CreateAsync(accountHistory);
        _logger.LogInformation($"{nameof(AccountHistory)} was successfully added");

        return new RegistrationResponse { Tokens = _jwtManager.Authenticate(user) };
    }
}
