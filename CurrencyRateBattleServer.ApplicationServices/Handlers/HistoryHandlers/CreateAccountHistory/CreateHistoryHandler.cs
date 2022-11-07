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

    private readonly IAccountService _accountService;

    private readonly IRoomService _roomService;

    private readonly IAccountHistoryService _accountHistoryService;

    public CreateHistoryHandler(ILogger<CreateHistoryHandler> logger, IAccountService accountService,
        IRoomService roomService, IAccountHistoryService accountHistoryService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
        _accountHistoryService = accountHistoryService ?? throw new ArgumentNullException(nameof(accountHistoryService));
    }

    public async Task<Result<CreateHistoryResponse>> Handle(CreateHistoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateHistoryHandler)} was caused... Start proccesing");
        
        if (request.UserId is null)
            return Result.Failure<CreateHistoryResponse>("Incorrect data.");

        var account = await _accountService.FindAsync(request.UserId);

        if (account is null)
            return Result.Failure<CreateHistoryResponse>("Account didn't found.");

        var room = await _roomService.FindAsync(request.AccountHistory.RoomId);

        if (room is null)
            return Result.Failure<CreateHistoryResponse>("Room didn't found.");

        var accountHistory = request.AccountHistory.ToDomain();

        accountHistory.AddAccount(account);
        accountHistory.AddRoom(room.ToDomain());
        
        await _accountHistoryService.CreateAsync(accountHistory);

        return new CreateHistoryResponse();
    }
}
