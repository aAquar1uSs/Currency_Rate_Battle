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

        var userEmailResult = Email.TryCreate(request.UserEmail);
        if (userEmailResult.IsFailure)
            return Result.Failure<GetUserBetsResponse>(userEmailResult.Error);
        
        var account = await _accountRepository.GetAccountByUserIdAsync(userEmailResult.Value, cancellationToken);
        if (account is null)
            return Result.Failure<GetUserBetsResponse>($"Account with such user id {request.UserEmail} does not exist");

        var bets = await _userRatingQueryRepository.FindAsync(account.Id, cancellationToken);

        return new GetUserBetsResponse { Bets = bets.ToDto() };
    }
}
