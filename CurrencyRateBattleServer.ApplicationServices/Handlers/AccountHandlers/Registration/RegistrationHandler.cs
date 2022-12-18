﻿using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager;
using CurrencyRateBattleServer.ApplicationServices.Infrastructure.JwtManager.Interfaces;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
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

        var customUserId = UserId.GenerateId();
        var userResult = User.TryCreate(customUserId.Id, request.UserDto.Email, request.UserDto.Password, null);

        if (userResult.IsFailure)
            return Result.Failure<RegistrationResponse>(userResult.Error);

        var user = userResult.Value;
        var maybeUser = await _userRepository.GetAsync(user, cancellationToken);

        if (maybeUser is not null)
            return Result.Failure<RegistrationResponse>("User with such email already exist");

        var customAccountId = AccountId.GenerateId();
        var accountResult = Account.TryCreateNewAccount(customAccountId.Id, customAccountId.Id);
        if (accountResult.IsFailure)
            return Result.Failure<RegistrationResponse>(accountResult.Error);
        var account = accountResult.Value;

        var amountResult = Amount.TryCreate(_options.RegistrationReward);
        if (amountResult.IsFailure)
            return Result.Failure<RegistrationResponse>(amountResult.Error);

        accountResult.Value.AddStartBalance(amountResult.Value);

        await _accountRepository.CreateAccountAsync(account, cancellationToken);

        await _userRepository.CreateAsync(user, cancellationToken);

        var accountHistoryId = AccountHistoryId.GenerateId();
        var accountHistory = AccountHistory.Create(accountHistoryId.Id, account.Id.Id, DateTime.Now, account.Amount.Value);
        await _accountHistoryRepository.CreateAsync(accountHistory, cancellationToken);

        return new RegistrationResponse { Tokens = _jwtManager.Authenticate(user) };
    }
}
