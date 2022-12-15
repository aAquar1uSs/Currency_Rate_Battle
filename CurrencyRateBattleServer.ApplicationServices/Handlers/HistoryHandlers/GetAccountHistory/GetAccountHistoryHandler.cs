using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;

public class GetAccountHistoryHandler : IRequestHandler<GetAccountHistoryCommand, Result<GetAccountHistoryResponse>>
{
    private readonly ILogger<GetAccountHistoryHandler> _logger;

    private readonly IAccountRepository _accountRepository;

    private readonly IAccountHistoryRepository _accountHistoryRepository;

    public GetAccountHistoryHandler(ILogger<GetAccountHistoryHandler> logger, IAccountRepository accountRepository,
        IAccountHistoryRepository accountHistoryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _accountHistoryRepository = accountHistoryRepository ?? throw new ArgumentNullException(nameof(accountHistoryRepository));
    }

    public async Task<Result<GetAccountHistoryResponse>> Handle(GetAccountHistoryCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId is null)
            return Result.Failure<GetAccountHistoryResponse>("User id is null.");
        
        var account = await _accountRepository.GetAccountByUserIdAsync(request.UserId);
        
        if (account is null)
            return Result.Failure<GetAccountHistoryResponse>("Account not found.");

        var history = await _accountHistoryRepository.GetAsync(request.UserId);

        return new GetAccountHistoryResponse { AccountHistories = history.ToArray().ToDto() };
    }
}
