using System.Transactions;
using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.ApplicationServices.Converters;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.Errors;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;

public class MakeBetHandler : IRequestHandler<MakeBetCommand, Result<MakeBetResponse, Error>>
{
    private readonly ILogger<MakeBetHandler> _logger;
    private readonly IAccountQueryRepository _accountQueryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IRateRepository _rateRepository;
    private readonly IRoomRepository _roomRepository;

    public MakeBetHandler(ILogger<MakeBetHandler> logger, IAccountQueryRepository accountQueryRepository,
        IRoomRepository roomRepository, IRateRepository rateRepository, IAccountRepository accountRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountQueryRepository = accountQueryRepository ?? throw new ArgumentNullException(nameof(accountQueryRepository));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<Result<MakeBetResponse, Error>> Handle(MakeBetCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(MakeBetHandler)} was caused.");
        var userEmailResult = Email.TryCreate(request.UserEmail);
        if (userEmailResult.IsFailure)
            return new PlayerValidationError("email_not_valid", userEmailResult.Error); 
        
        var account = await _accountQueryRepository.GetAccountByUserId(userEmailResult.Value, cancellationToken);
        if (account is null)
            return PlayerValidationError.AccountNotFound;
        
        var rateToCreate = request.UserRateToCreate;
        var roomIdResult = RoomId.TryCreate(rateToCreate.RoomId);
        if (roomIdResult.IsFailure)
            return new RateValidationError("invalid_rate" ,roomIdResult.Error);

        var room = await _roomRepository.Find(roomIdResult.Value.Id, cancellationToken);
        if (room is null)
            return RoomValidationError.NotFound;

        var amountResult = Amount.TryCreate(rateToCreate.Amount);
        if (amountResult.IsFailure)
            return new MoneyValidationError("invalid_amount" ,amountResult.Error);

        var result = account.WritingOffMoney(amountResult.Value);
        if (result.IsFailure)
            return new MoneyValidationError("writing_off_error" ,result.Error);

        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _accountRepository.Update(account, cancellationToken);

        var incrementResult = room.IncrementCountRates();
        if (incrementResult.IsFailure)
            return new CommonError("increment_count_of_rates_error", incrementResult.Error);

        var rate = Rate.Create(CustomId.GenerateId().Id, DateTime.UtcNow, rateToCreate.UserCurrencyExchange,
            rateToCreate.Amount, null, null, false, false, roomIdResult.Value.Id,
            room.CurrencyName.Value, account.Id.Id);

        await _rateRepository.Create(rate, cancellationToken);
        await _roomRepository.Update(room, cancellationToken);

        transactionScope.Complete();
        
        return new MakeBetResponse { UserRate = rate.ToDto() };
    }
}
