using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;

public class CreateHistoryHandler : IRequestHandler<CreateHistoryCommand, Result<CreateHistoryResponse>>
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

    public async Task<Result<CreateHistoryResponse>> Handle(CreateHistoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateHistoryHandler)} was caused... Start proccesing");

        var userIdResult = UserId.TryCreate(request.UserId);
        if (userIdResult.IsFailure)
            return Result.Failure<CreateHistoryResponse>(userIdResult.Error);
        
        var account = await _accountRepository.GetAccountByUserIdAsync(userIdResult.Value, cancellationToken);

        if (account is null)
            return Result.Failure<CreateHistoryResponse>("Account didn't found.");

        var roomIdResult = RoomId.TryCreate(request.AccountHistory.RoomId);
        if (roomIdResult.IsFailure)
            return Result.Failure<CreateHistoryResponse>(roomIdResult.Error);
        
        var room = await _roomRepository.FindAsync(roomIdResult.Value.Id, cancellationToken);

        if (room is null)
            return Result.Failure<CreateHistoryResponse>("Room didn't found.");

        var customAccountHistoryId = AccountHistoryId.GenerateId();
        var accountHistory = AccountHistory.Create(customAccountHistoryId.Id, account.Id.Id, DateTime.Now, account.Amount.Value);

        await _accountHistoryRepository.CreateAsync(accountHistory, cancellationToken);

        return new CreateHistoryResponse();
    }
}
