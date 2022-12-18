using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;

public class GetUserBetsHandler : IRequestHandler<GetUserBetsCommand, Result<GetUserBetsResponse>>
{
    private readonly ILogger<GetUserBetsHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRatingQueryRepository _userRatingQueryRepository;

    public GetUserBetsHandler(ILogger<GetUserBetsHandler> logger, IAccountRepository accountRepository,
        IUserRatingQueryRepository userRatingQueryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _userRatingQueryRepository = userRatingQueryRepository ?? throw new ArgumentNullException(nameof(userRatingQueryRepository));
    }

    public async Task<Result<GetUserBetsResponse>> Handle(GetUserBetsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetUserBetsHandler)},  was caused. Start processing.");

        var userIdResult = UserId.TryCreate(request.UserId);
        if (userIdResult.IsFailure)
            return Result.Failure<GetUserBetsResponse>(userIdResult.Error);
        
        var account = await _accountRepository.GetAccountByUserIdAsync(userIdResult.Value, cancellationToken);
        if (account is null)
            return Result.Failure<GetUserBetsResponse>($"Account with such user id {request.UserId} does not exist");

        var bets = await _userRatingQueryRepository.FindAsync(account.Id, cancellationToken);

        return new GetUserBetsResponse { Bets = bets.ToDto() };
    }
}
