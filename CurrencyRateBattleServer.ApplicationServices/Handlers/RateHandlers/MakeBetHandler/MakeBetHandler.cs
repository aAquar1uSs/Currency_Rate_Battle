using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.MakeBetHandler;

public class MakeBetHandler : IRequestHandler<MakeBetCommand, Result<MakeBetResponse>>
{
    private readonly ILogger<MakeBetHandler> _logger;

    private readonly IAccountService _accountService;

    private readonly IPaymentService _paymentService;

    private readonly ICurrencyStateService _currencyStateService;

    private readonly IRateService _rateService;

    public MakeBetHandler(ILogger<MakeBetHandler> logger, IAccountService accountService, IPaymentService paymentService,
        ICurrencyStateService currencyStateService, IRateService rateService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _currencyStateService = currencyStateService ?? throw new ArgumentNullException(nameof(currencyStateService));
        _rateService = rateService ?? throw new ArgumentNullException(nameof(rateService));
    }

    public async Task<Result<MakeBetResponse>> Handle(MakeBetCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(MakeBetHandler)} was caused.");
        var account = await _accountService.GetAccountByUserIdAsync(request.UserId);

        if (account is null)
            return Result.Failure<MakeBetResponse>($"Account with such user id: {request.UserId} does not exist.");

        var rateToCreate = request.RateToCreate.ToDomain();

        var result = await _paymentService.WritingOffMoneyAsync(account, rateToCreate.Amount);

        if (result is false)
            return Result.Failure<MakeBetResponse>("Payment processing error");

        var currencyId = await _currencyStateService.GetCurrencyIdByRoomIdAsync(rateToCreate.RoomId);

        if (currencyId == Guid.Empty)
            return Result.Failure<MakeBetResponse>("Incorrect data");

        var rate = await _rateService.CreateRateAsync(rateToCreate, account.Id, currencyId);

        return new MakeBetResponse { Rate = rate.ToDto() };
    }
}
