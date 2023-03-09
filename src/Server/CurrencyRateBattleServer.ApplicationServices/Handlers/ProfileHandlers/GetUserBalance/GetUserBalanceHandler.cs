using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.ProfileHandlers.GetUserBalance;

public class GetUserBalanceHandler : IRequestHandler<GetUserBalanceCommand, Result<GetUserBalanceResponse, Error>>
{
    private readonly ILogger<GetUserBalanceHandler> _logger;
    private readonly IAccountRepository _accountRepository;

    public GetUserBalanceHandler(ILogger<GetUserBalanceHandler> logger, IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Result<GetUserBalanceResponse, Error>> Handle(GetUserBalanceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(GetUserBalanceHandler)} was caused.");

        var userEmailResult = Email.TryCreate(request.UserId);
        if (userEmailResult.IsFailure)
            return new PlayerValidationError("Invalid_email", userEmailResult.Error);
        
        var account = await _accountRepository.GetAccountByUserIdAsync(userEmailResult.Value, cancellationToken);

        if (account is null)
            return PlayerValidationError.AccountNotFound;

        return new GetUserBalanceResponse { Amount = account.Amount.Value };
    }
}
