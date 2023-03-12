using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;

public class CreateHistoryHandler : IRequestHandler<CreateHistoryCommand, Maybe<Error>>
{
    private readonly ILogger<CreateHistoryHandler> _logger;
    private readonly IAccountQueryRepository _accountQueryRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IAccountHistoryRepository _accountHistoryRepository;

    public CreateHistoryHandler(ILogger<CreateHistoryHandler> logger, IAccountQueryRepository accountQueryRepository,
        IRoomRepository roomRepository, IAccountHistoryRepository accountHistoryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountQueryRepository = accountQueryRepository ?? throw new ArgumentNullException(nameof(accountQueryRepository));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _accountHistoryRepository = accountHistoryRepository ?? throw new ArgumentNullException(nameof(accountHistoryRepository));
    }

    public async Task<Maybe<Error>> Handle(CreateHistoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateHistoryHandler)} was caused... Start processing");

        var emailResult = Email.TryCreate(request.UserEmail);
        if (emailResult.IsFailure)
            return new PlayerValidationError("email_not_valid", emailResult.Error);

        var account = await _accountQueryRepository.GetAccountByUserId(emailResult.Value, cancellationToken);

        if (account is null)
            return PlayerValidationError.AccountNotFound;

        var customAccountHistoryId = AccountHistoryId.GenerateId();

        var result = request.RoomId is null
            ? await CreateWithoutRoom(customAccountHistoryId.Id, account.Id.Id, request.Amount, request.IsCredit, cancellationToken)
            : await CreateWithRoom(customAccountHistoryId.Id, account.Id.Id, request.Amount, request.IsCredit, request.RoomId.Value,
                cancellationToken);

        return result.HasValue 
            ? result.Value
            : Maybe<Error>.None;
    }

    private async Task<Maybe<Error>> CreateWithoutRoom(Guid historyId, Guid accId, decimal amount, bool isCredit, CancellationToken cancellationToken)
    {
       var accountHistory = AccountHistory.Create(historyId, accId, DateTime.UtcNow, amount, isCredit);
       await _accountHistoryRepository.Create(accountHistory, cancellationToken);

       return Maybe<Error>.None;
    }

    private async Task<Maybe<Error>> CreateWithRoom(Guid historyId, Guid accId, decimal amount, bool isCredit, Guid roomId, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.Find(roomId, cancellationToken);
        if (room is null)
            return RoomValidationError.NotFound;

        var accountHistory = AccountHistory.Create(historyId, accId, DateTime.Now, amount, isCredit, room.Id.Id);
        await _accountHistoryRepository.Create(accountHistory, cancellationToken);

        return Maybe<Error>.None;
    }
}
