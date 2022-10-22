using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetUserBalance;

public class GetUserBalanceHandler : IRequestHandler<GetUserBalanceCommand, Result<GetUserBalanceResponse>>
{
    private readonly ILogger<GetUserBalanceHandler> _logger;

    private readonly IAccountService _accountService;

    public GetUserBalanceHandler(ILogger<GetUserBalanceHandler> logger, IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Result<GetUserBalanceResponse>> Handle(GetUserBalanceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetUserBalanceHandler)} was caused.");
        
        if (request.UserId is null)
            return Result.Failure<GetUserBalanceResponse>("User id is null.");
        
        var account = await _accountService.GetAccountByUserIdAsync(request.UserId);
        
        if (account is null)
            return Result.Failure<GetUserBalanceResponse>($"Account with id: {request.UserId} didn't found.");

        return new GetUserBalanceResponse { Amount = account.Amount };
    }
}
