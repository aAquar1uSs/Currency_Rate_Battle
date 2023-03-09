using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using CurrencyRateBattleServer.Dto;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetProfile;

public class GetProfileHandler : IRequestHandler<GetProfileCommand, Result<GetProfileResponse, Error>>
{
    private readonly ILogger<GetProfileHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;

    public GetProfileHandler(ILogger<GetProfileHandler> logger, IAccountRepository accountRepository, IUserRepository userRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<GetProfileResponse, Error>> Handle(GetProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetProfileHandler)} was caused.");

        var userEmailResult = Email.TryCreate(request.UserEmail);
        if (userEmailResult.IsFailure)
            return new PlayerValidationError("Invalid_email", userEmailResult.Error);
        
        var user = await _userRepository.FindAsync(userEmailResult.Value, cancellationToken);
        if (user is null)
            return PlayerValidationError.UserNotFound;
        
        var account = await _accountRepository.GetAccountByUserIdAsync(userEmailResult.Value, cancellationToken);
        if (account is null)
            return PlayerValidationError.AccountNotFound;

        return new GetProfileResponse
        {
            AccountInfo = new AccountInfoDto { Amount = account.Amount.Value, Email = user.Email.Value }
        };
    }
}
