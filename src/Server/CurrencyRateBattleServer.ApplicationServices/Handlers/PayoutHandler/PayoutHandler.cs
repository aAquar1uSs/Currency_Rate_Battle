using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.PayoutHandler;

public class PayoutHandler : IRequestHandler<PayoutCommand, Maybe<Error>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<PayoutHandler> _logger;

    public PayoutHandler(IAccountRepository accountRepository, IMediator mediator, ILogger<PayoutHandler> logger)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Maybe<Error>> Handle(PayoutCommand request, CancellationToken cancellationToken)
    {
        var accountIdResult = AccountId.TryCreate(request.AccountId);
        var account = await _accountRepository.Get(accountIdResult.Value, cancellationToken);
        if (account is null)
        {
            _logger.LogWarning("Account not found when rate was processed. Skip processing for account id: {Id}", request.AccountId);
            return PlayerValidationError.AccountNotFound;
        }

        var moneyCreatedResult = Amount.TryCreate(request.Payout);
        if (moneyCreatedResult.IsFailure)
        {
            _logger.LogWarning("Failed to payment process. For account id: {id}. Skip processing.Reason: {Reason}", account.Id.Value, moneyCreatedResult.Error);
            return new MoneyValidationError("amount_not_valid", moneyCreatedResult.Error);
        }

        account.ApportionCash(moneyCreatedResult.Value);

        await _accountRepository.Update(account, cancellationToken);

        var command = new CreateHistoryCommand(account.UserEmail.Value, request.RoomId, DateTime.UtcNow, moneyCreatedResult.Value.Value, true);
        _ = await _mediator.Send(command, cancellationToken);
        
        return Maybe<Error>.None;
    } 
}
