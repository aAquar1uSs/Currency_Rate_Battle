using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;

public class MakeBetHandler : IRequestHandler<MakeBetCommand, Result<MakeBetResponse>>
{
    private readonly ILogger<MakeBetHandler> _logger;

    private readonly IAccountRepository _accountRepository;

    private readonly IPaymentRepository _paymentRepository;

    private readonly ICurrencyStateRepository _currencyStateRepository;

    private readonly IRateRepository _rateRepository;

    public MakeBetHandler(ILogger<MakeBetHandler> logger, IAccountRepository accountRepository, IPaymentRepository paymentRepository,
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
        var account = await _accountRepository.GetAccountByUserIdAsync(request.UserId);

        if (account is null)
            return Result.Failure<MakeBetResponse>($"Account with such user id: {request.UserId} does not exist.");

        var rateToCreate = request.RateToCreate.ToDomain();

        var result = await _paymentRepository.WritingOffMoneyAsync(account, rateToCreate.Amount);

        if (result is false)
            return Result.Failure<MakeBetResponse>("Payment processing error");

        var currencyId = await _currencyStateRepository.GetCurrencyIdByRoomIdAsync(rateToCreate.RoomId);

        if (currencyId == Guid.Empty)
            return Result.Failure<MakeBetResponse>("Incorrect data");

        var rate = await _rateRepository.CreateRateAsync(rateToCreate, account.Id, currencyId);

        return new MakeBetResponse { Rate = rate.ToDto() };
    }
}
