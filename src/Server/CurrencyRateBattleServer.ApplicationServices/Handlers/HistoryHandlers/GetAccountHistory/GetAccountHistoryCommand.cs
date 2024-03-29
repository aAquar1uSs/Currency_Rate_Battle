﻿using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.GetAccountHistory;

public class GetAccountHistoryCommand : IRequest<Result<GetAccountHistoryResponse, Error>>
{
    public GetAccountHistoryCommand(string userEmail)
    {
        UserEmail = userEmail;
    }
    
    public string UserEmail { get; }
}
