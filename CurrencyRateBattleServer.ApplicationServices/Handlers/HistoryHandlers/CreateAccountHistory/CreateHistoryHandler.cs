using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Converters;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
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
        
        if (request.UserId is null)
            return Result.Failure<CreateHistoryResponse>("Incorrect data.");

        var account = await _accountRepository.GetAccountByUserIdAsync(request.UserId);

        if (account is null)
            return Result.Failure<CreateHistoryResponse>("Account didn't found.");

        var room = await _roomRepository.GetRoomByIdAsync(request.AccountHistory.RoomId);

        if (room is null)
            return Result.Failure<CreateHistoryResponse>("Room didn't found.");

        var accountHistory = request.AccountHistory.ToDomain();

        accountHistory.AddAccount(account);
        accountHistory.AddRoom(room.ToDomain());
        
        await _accountHistoryRepository.CreateAsync(accountHistory);

        return new CreateHistoryResponse();
    }
}
