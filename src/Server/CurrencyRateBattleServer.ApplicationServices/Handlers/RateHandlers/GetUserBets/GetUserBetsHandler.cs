using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;

public class GetUserBetsHandler : IRequestHandler<GetUserBetsCommand, Result<GetUserBetsResponse, Error>>
{
    private readonly ILogger<GetUserBetsHandler> _logger;
    private readonly IAccountQueryRepository _accountQueryRepository;
    private readonly IUserRatingQueryRepository _userRatingQueryRepository;

    public GetUserBetsHandler(ILogger<GetUserBetsHandler> logger, IAccountQueryRepository accountQueryRepository,
        IUserRatingQueryRepository userRatingQueryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountQueryRepository = accountQueryRepository ?? throw new ArgumentNullException(nameof(accountQueryRepository));
        _userRatingQueryRepository = userRatingQueryRepository ?? throw new ArgumentNullException(nameof(userRatingQueryRepository));
    }

    public async Task<Result<GetUserBetsResponse, Error>> Handle(GetUserBetsCommand request, CancellationToken cancellationToken)
    {
        var userEmailResult = Email.TryCreate(request.UserEmail);
        if (userEmailResult.IsFailure)
            return new PlayerValidationError("email_not_valid", userEmailResult.Error); 
        
        var account = await _accountQueryRepository.GetAccountByUserId(userEmailResult.Value, cancellationToken);
        if (account is null)
        {
            _logger.LogWarning("Account with such user email: {Email} not found.", userEmailResult.Value);
            return PlayerValidationError.AccountNotFound;
        }

        var bets = await _userRatingQueryRepository.FindAsync(account.Id, cancellationToken);

        return new GetUserBetsResponse { Bets = bets.Select(x => x.ToDto()).ToArray() };
    }
}
