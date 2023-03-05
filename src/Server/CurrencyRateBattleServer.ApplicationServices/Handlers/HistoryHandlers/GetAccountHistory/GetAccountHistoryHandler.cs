using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;

public class GetAccountHistoryHandler : IRequestHandler<GetAccountHistoryCommand, Result<GetAccountHistoryResponse, Error>>
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

    public async Task<Result<GetAccountHistoryResponse, Error>> Handle(GetAccountHistoryCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.TryCreate(request.UserEmail);
        if (emailResult.IsFailure)
            return new PlayerValidationError("email_not_valid", emailResult.Error);
        var email = emailResult.Value;

        var account = await _accountRepository.GetAccountByUserIdAsync(email, cancellationToken);

        if (account is null)
            return PlayerValidationError.AccountNotFound;

        var history = await _accountHistoryRepository.GetAsync(account.Id, cancellationToken);

        return new GetAccountHistoryResponse { AccountHistories = history.ToArray().ToDto() };
    }
}
