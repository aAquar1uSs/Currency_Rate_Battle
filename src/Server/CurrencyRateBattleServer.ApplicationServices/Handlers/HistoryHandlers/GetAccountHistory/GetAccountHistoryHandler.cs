using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
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
        var userIdResult = UserId.TryCreate(request.UserId);
        if (userIdResult.IsFailure)
            return Result.Failure<GetAccountHistoryResponse>(userIdResult.Error);
        var userId = userIdResult.Value;
        
        var account = await _accountRepository.GetAccountByUserIdAsync(userId, cancellationToken);
        
        if (account is null)
            return Result.Failure<GetAccountHistoryResponse>("Account not found.");

        var history = await _accountHistoryRepository.GetAsync(account.Id, cancellationToken);

        return new GetAccountHistoryResponse { AccountHistories = history.ToArray().ToDto() };
    }
}
