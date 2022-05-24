using System.Runtime.CompilerServices;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Services.HostedServices.Handlers;
using CurrencyRateBattleServer.Services.Interfaces;

namespace CurrencyRateBattleServer.Services;

public class RateCalculationService : IRateCalculationService
{
    private readonly ILogger<RateCalculationService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IRateService _rateService;

    private readonly IPaymentService _paymentService;

    private readonly WinnerHandler _winnerHandler;

    private readonly PayoutHandler _payoutHandler;

    public RateCalculationService(ILogger<RateCalculationService> logger,
        IServiceScopeFactory scopeFactory,
        IRateService rateService,
        IPaymentService paymentService)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _rateService = rateService;
        _paymentService = paymentService;
        _winnerHandler = new WinnerHandler(_scopeFactory);
        _payoutHandler = new PayoutHandler();

        _winnerHandler.SetNext(_payoutHandler);
    }

    public async Task StartRateCalculationByRoomIdAsync(Guid roomId)
    {
        var rates = await _rateService.GetRateByRoomIdAsync(roomId);

        if (rates.Count == 0)
            return;

        try
        {
            var updatedRate = await _winnerHandler.Handle(rates);

            foreach (var rate in updatedRate)
            {
                await _paymentService.ApportionCashByRateAsync(rate.Id, rate.Payout);
                await _rateService.UpdateRateByRoomIdAsync(roomId, rate);
            }
        }
        catch (CustomException)
        {
            _logger.LogDebug("There was 1 bet in the room. Cashback...");
            await _paymentService.ApportionCashByRateAsync(rates.First().AccountId, rates.First().Amount);
        }
    }
}
