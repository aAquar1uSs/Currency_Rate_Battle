using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using CurrencyRateBattleServer.Dto;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetProfile;

public class ProfileHandler : IRequestHandler<GetProfileCommand, Result<GetProfileResponse>>
{
    private readonly ILogger<ProfileHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;

    public ProfileHandler(ILogger<ProfileHandler> logger, IAccountRepository accountRepository, IUserRepository userRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<GetProfileResponse>> Handle(GetProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(ProfileHandler)} was caused.");

        var userIdResult = UserId.TryCreate(request.UserId);
        if (userIdResult.IsFailure)
            return Result.Failure<GetProfileResponse>(userIdResult.Error);
        
        var user = await _userRepository.FindAsync(userIdResult.Value, cancellationToken);
        if (user is null)
            return Result.Failure<GetProfileResponse>($"User with suh id: {request.UserId} didn't found,");
        
        var account = await _accountRepository.GetAccountByUserIdAsync(userIdResult.Value, cancellationToken);
        if (account is null)
            return Result.Failure<GetProfileResponse>($"Account with such user id: {request.UserId} didn't found,");

        return new GetProfileResponse
        {
            AccountInfo = new AccountInfoDto { Amount = account.Amount.Value, Email = user.Email.Value }
        };
    }
}
