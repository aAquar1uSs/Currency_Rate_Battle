using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;

public class GetUserBetsHandler : IRequestHandler<GetUserBetsCommand, Result<GetUserBetsResponse>>
{
    private readonly ILogger<GetUserBetsHandler> _logger;

    private readonly IAccountService _accountService;

    private readonly IRateService _rateService;

    public GetUserBetsHandler(ILogger<GetUserBetsHandler> logger, IAccountService accountService, IRateService rateService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _rateService = rateService ?? throw new ArgumentNullException(nameof(rateService));
    }

    public async Task<Result<GetUserBetsResponse>> Handle(GetUserBetsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetUserBetsHandler)},  was caused. Start processing.");

        var account = await _accountService.GetAccountByUserIdAsync(request.UserId);

        if (account is null)
            return Result.Failure<GetUserBetsResponse>($"Account with such user id {request.UserId} does not exist");

        var bets = await _rateService.GetRatesByAccountIdAsync(account.Id);

        return new GetUserBetsResponse { Bets = bets)};
    }
}
