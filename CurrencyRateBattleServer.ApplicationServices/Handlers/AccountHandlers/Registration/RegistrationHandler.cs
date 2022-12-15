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
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly WebServerOptions _options;
    private readonly IAccountHistoryRepository _accountHistoryRepository;
    private readonly IJwtManager _jwtManager;

    public RegistrationHandler(ILogger<RegistrationHandler> logger, IAccountRepository accountRepository, 
        IOptions<WebServerOptions> options, IAccountHistoryRepository historyRepository, IUserRepository userRepository,
        JwtManager jwtManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _accountHistoryRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
        _jwtManager = jwtManager ?? throw new ArgumentNullException(nameof(jwtManager));
    }

    public async Task<Result<RegistrationResponse>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(RegistrationHandler)} was caused.");

        var userResult = User.Create(request.UserDto.Email, request.UserDto.Password);

        if (userResult.IsFailure)
            return Result.Failure<RegistrationResponse>(userResult.Error);

        var user = userResult.Value;
        var maybeUser = await _userRepository.GetAsync(user, cancellationToken);

        if (maybeUser is not null)
            return Result.Failure<RegistrationResponse>("User with such email already exist");

        var account = Account.Create();
        account.AddStartBalance(_options.RegistrationReward);

        await _accountRepository.CreateAccountAsync(account);

        user.AddAccount(account);

        await _userRepository.CreateAsync(user, cancellationToken);

        var accountHistory = AccountHistory.Create(account.Id, DateTime.UtcNow, account.Amount, true);
        await _accountHistoryRepository.CreateAsync(accountHistory, cancellationToken);

        return new RegistrationResponse { Tokens = _jwtManager.Authenticate(user) };
    }
}
