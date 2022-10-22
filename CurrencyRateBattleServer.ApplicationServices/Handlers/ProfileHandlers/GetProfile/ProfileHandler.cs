using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Dto;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetProfile;

public class ProfileHandler : IRequestHandler<GetProfileCommand, Result<GetProfileResponse>>
{
    private readonly ILogger<ProfileHandler> _logger;

    private readonly IAccountService _accountService;

    public ProfileHandler(ILogger<ProfileHandler> logger, IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<GetProfileResponse>> Handle(GetProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(ProfileHandler)} was caused.");

        if (request.UserId is null)
            return Result.Failure<GetProfileResponse>("User id is null.");
        
        var account = await _accountService.GetAccountByUserIdAsync(request.UserId);
        
        if (account is null)
            return Result.Failure<GetProfileResponse>($"User with suh id: {request.UserId} didn't found,");

        return new GetProfileResponse
        {
            AccountInfo = new AccountInfoDto { Amount = account.Amount, Email = account.User.Email }
        };
    }
}
