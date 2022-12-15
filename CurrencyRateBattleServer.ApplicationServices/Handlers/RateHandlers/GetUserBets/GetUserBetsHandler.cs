using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;

public class GetUserBetsHandler : IRequestHandler<GetUserBetsCommand, Result<GetUserBetsResponse>>
{
    private readonly ILogger<GetUserBetsHandler> _logger;

    private readonly IAccountRepository _accountRepository;

    private readonly IRateRepository _rateRepository;

    public GetUserBetsHandler(ILogger<GetUserBetsHandler> logger, IAccountRepository accountRepository, IRateRepository rateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
    }

    public async Task<Result<GetUserBetsResponse>> Handle(GetUserBetsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetUserBetsHandler)},  was caused. Start processing.");

        var account = await _accountRepository.GetAccountByUserIdAsync(request.UserId);

        if (account is null)
            return Result.Failure<GetUserBetsResponse>($"Account with such user id {request.UserId} does not exist");

        var bets = await _rateRepository.GetRatesByAccountIdAsync(account.Id);

        return new GetUserBetsResponse { Bets = bets)};
    }
}
