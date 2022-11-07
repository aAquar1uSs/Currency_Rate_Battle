using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;

public class GetAccountHistoryHandler : IRequestHandler<GetAccountHistoryCommand, Result<GetAccountHistoryResponse>>
{
    private readonly ILogger<GetAccountHistoryHandler> _logger;

    private readonly IAccountService _accountService;

    private readonly IAccountHistoryService _accountHistoryService;

    public GetAccountHistoryHandler(ILogger<GetAccountHistoryHandler> logger, IAccountService accountService,
        IAccountHistoryService accountHistoryService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _accountHistoryService = accountHistoryService ?? throw new ArgumentNullException(nameof(accountHistoryService));
    }

    public async Task<Result<GetAccountHistoryResponse>> Handle(GetAccountHistoryCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId is null)
            return Result.Failure<GetAccountHistoryResponse>("User id is null.");
        
        var account = await _accountService.FindAsync(request.UserId);
        
        if (account is null)
            return Result.Failure<GetAccountHistoryResponse>("Account not found.");

        var history = await _accountHistoryService.FindAsync(request.UserId);

        return new GetAccountHistoryResponse { AccountHistories = history.ToArray().ToDto() };
    }
}
