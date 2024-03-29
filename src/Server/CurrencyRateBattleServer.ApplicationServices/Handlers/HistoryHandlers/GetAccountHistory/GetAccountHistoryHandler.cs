﻿using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;

public class GetAccountHistoryHandler : IRequestHandler<GetAccountHistoryCommand, Result<GetAccountHistoryResponse, Error>>
{
    private readonly IAccountQueryRepository _accountQueryRepository;
    private readonly IAccountHistoryRepository _accountHistoryRepository;

    public GetAccountHistoryHandler(IAccountQueryRepository accountQueryRepository,
        IAccountHistoryRepository accountHistoryRepository)
    {
        _accountQueryRepository = accountQueryRepository ?? throw new ArgumentNullException(nameof(accountQueryRepository));
        _accountHistoryRepository = accountHistoryRepository ?? throw new ArgumentNullException(nameof(accountHistoryRepository));
    }

    public async Task<Result<GetAccountHistoryResponse, Error>> Handle(GetAccountHistoryCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.TryCreate(request.UserEmail);
        if (emailResult.IsFailure)
            return new PlayerValidationError("email_not_valid", emailResult.Error);
        var email = emailResult.Value;

        var account = await _accountQueryRepository.GetAccountByUserId(email, cancellationToken);

        if (account is null)
            return PlayerValidationError.AccountNotFound;

        var history = await _accountHistoryRepository.Get(account.Id, cancellationToken);

        return new GetAccountHistoryResponse { AccountHistories = history.Select(x => x.ToDto()).ToArray() };
    }
}
