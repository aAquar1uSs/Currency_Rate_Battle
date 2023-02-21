using System.Transactions;
using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;

public class MakeBetHandler : IRequestHandler<MakeBetCommand, Result<MakeBetResponse>>
{
    private readonly ILogger<MakeBetHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IRateRepository _rateRepository;
    private readonly IRoomRepository _roomRepository;

    public MakeBetHandler(ILogger<MakeBetHandler> logger, IAccountRepository accountRepository,
        IRoomRepository roomRepository, IRateRepository rateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
    }

    public async Task<Result<MakeBetResponse>> Handle(MakeBetCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(MakeBetHandler)} was caused.");
        var userIdResult = UserId.TryCreate(request.UserId);
        if (userIdResult.IsFailure)
            return Result.Failure<MakeBetResponse>(userIdResult.Error);
        
        var account = await _accountRepository.GetAccountByUserIdAsync(userIdResult.Value, cancellationToken);
        if (account is null)
            return Result.Failure<MakeBetResponse>($"Account with such user id: {request.UserId} does not exist.");
        
        var rateToCreate = request.UserRateToCreate;
        var roomIdResult = RoomId.TryCreate(rateToCreate.RoomId);
        if (roomIdResult.IsFailure)
            return Result.Failure<MakeBetResponse>(roomIdResult.Error);

        var room = await _roomRepository.FindAsync(roomIdResult.Value.Id, cancellationToken);
        if (room is null)
            return Result.Failure<MakeBetResponse>("Room not found.");

        var amountResult = Amount.TryCreate(rateToCreate.Amount);
        if (amountResult.IsFailure)
            return Result.Failure<MakeBetResponse>(amountResult.Error);

        var result = account.WritingOffMoney(amountResult.Value);
        if (result.IsFailure)
            return Result.Failure<MakeBetResponse>(result.Error);

        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _accountRepository.UpdateAsync(account, cancellationToken);

        var incrementResult = room.IncrementCountRates();
        if (incrementResult.IsFailure)
            return Result.Failure<MakeBetResponse>(incrementResult.Error);
        
        var rate = Rate.Create(CustomId.GenerateId().Id, DateTime.UtcNow, rateToCreate.UserCurrencyExchange,
            rateToCreate.Amount, null, null, false, false, roomIdResult.Value.Id,
            room.CurrencyName.Value, account.Id.Id);

        await _rateRepository.CreateAsync(rate, cancellationToken);
        await _roomRepository.UpdateAsync(room, cancellationToken);

        transactionScope.Complete();
        
        return new MakeBetResponse { UserRate = rate.ToDto() };
    }
}
