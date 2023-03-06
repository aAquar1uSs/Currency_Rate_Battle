﻿using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;

public class CreateHistoryHandler : IRequestHandler<CreateHistoryCommand, Result<Unit, Error>>
{
    private readonly ILogger<CreateHistoryHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IAccountHistoryRepository _accountHistoryRepository;

    public CreateHistoryHandler(ILogger<CreateHistoryHandler> logger, IAccountRepository accountRepository,
        IRoomRepository roomRepository, IAccountHistoryRepository accountHistoryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _accountHistoryRepository = accountHistoryRepository ?? throw new ArgumentNullException(nameof(accountHistoryRepository));
    }

    public async Task<Result<Unit, Error>> Handle(CreateHistoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateHistoryHandler)} was caused... Start proccesing");

        var emailResult = Email.TryCreate(request.UserEmail);
        if (emailResult.IsFailure)
            return new PlayerValidationError("email_not_valid", emailResult.Error);
        
        var account = await _accountRepository.GetAccountByUserIdAsync(emailResult.Value, cancellationToken);

        if (account is null)
            return PlayerValidationError.AccountNotFound;
        
        var customAccountHistoryId = AccountHistoryId.GenerateId();

        var result = request.RoomId is null
            ? await CreateWithoutRoom(customAccountHistoryId.Id, account.Id.Id, account.Amount.Value, request.IsCredit, cancellationToken)
            : await CreateWithRoom(customAccountHistoryId.Id, account.Id.Id, account.Amount.Value, request.IsCredit, request.RoomId.Value,
                cancellationToken);
        
        return result.IsFailure 
            ? Result.Failure<Unit, Error>(result.Error) 
            : Unit.Value;
    }

    private async Task<Result<Unit, Error>> CreateWithoutRoom(Guid historyId, Guid accId, decimal amount, bool isCredit, CancellationToken cancellationToken)
    {
       var accountHistory = AccountHistory.Create(historyId, accId, DateTime.UtcNow, amount, isCredit);
       await _accountHistoryRepository.CreateAsync(accountHistory, cancellationToken);
       
       return Unit.Value;
    }
    
    private async Task<Result<Unit, Error>> CreateWithRoom(Guid historyId, Guid accId, decimal amount, bool isCredit, Guid roomId, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindAsync(roomId, cancellationToken);
        if (room is null)
            return RoomValidationError.RoomNotFound;
        
        var accountHistory = AccountHistory.Create(historyId, accId, DateTime.Now, amount, isCredit, room.Id.Id);
        await _accountHistoryRepository.CreateAsync(accountHistory, cancellationToken);

        return Unit.Value;
    }
}
