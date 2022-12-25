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
    private readonly ICurrencyStateRepository _currencyStateRepository;
    private readonly IRateRepository _rateRepository;

    public MakeBetHandler(ILogger<MakeBetHandler> logger, IAccountRepository accountRepository,
        ICurrencyStateRepository currencyStateRepository, IRateRepository rateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _currencyStateRepository = currencyStateRepository ?? throw new ArgumentNullException(nameof(currencyStateRepository));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
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
        
        var currencyId = await _currencyStateRepository.GetCurrencyStateIdByRoomIdAsync(roomIdResult.Value, cancellationToken);
        var currencyIdResult = CurrencyCode.TryCreate(currencyId);
        if (currencyIdResult.IsFailure)
            return Result.Failure<MakeBetResponse>(currencyIdResult.Error);
        
        var amountResult = Amount.TryCreate(rateToCreate.Amount);
        if (amountResult.IsFailure)
            return Result.Failure<MakeBetResponse>(amountResult.Error);

        var result = account.WritingOffMoney(amountResult.Value);
        if (result.IsFailure)
            return Result.Failure<MakeBetResponse>(result.Error);

        await _accountRepository.UpdateAsync(account, cancellationToken);

        var rate = Rate.Create(OneId.GenerateId().Id, DateTime.UtcNow, rateToCreate.UserCurrencyExchange,
            rateToCreate.Amount, null, null, false, false, roomIdResult.Value.Id,
            currencyIdResult.Value.Value, account.Id.Id);

        await _rateRepository.CreateAsync(rate, cancellationToken);

        return new MakeBetResponse { UserRate = rate.ToDto() };
    }
}
