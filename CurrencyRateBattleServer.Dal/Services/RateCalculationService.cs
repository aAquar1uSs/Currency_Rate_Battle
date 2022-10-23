using CurrencyRateBattleServer.Dal.Services.HostedServices.Handlers;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Services.HostedServices.Handlers;
using CurrencyRateBattleServer.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Services;

public class RateCalculationService : IRateCalculationService
{
    private readonly ILogger<RateCalculationService> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IRateService _rateService;

    private readonly IPaymentService _paymentService;

    private readonly WinnerHandler _winnerHandler;

    private readonly CalculationHandler _calculationHandler;

    public RateCalculationService(ILogger<RateCalculationService> logger,
        IServiceScopeFactory scopeFactory,
        IRateService rateService,
        IPaymentService paymentService)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _rateService = rateService;
        _paymentService = paymentService;

        //Chain of responsibility
        _winnerHandler = new WinnerHandler(_scopeFactory);
        _calculationHandler = new CalculationHandler();

        _ = _winnerHandler.SetNext(_calculationHandler);
    }

    public async Task StartRateCalculationByRoomIdAsync(Guid roomId)
    {
        _logger.LogInformation($"{nameof(StartRateCalculationByRoomIdAsync)} was caused.");
        var rates = await _rateService.GetRateByRoomIdAsync(roomId);

        if (rates.Count == 0)
            throw new GeneralException();

        if (rates.Any(r => r.IsClosed))
            return;

        try
        {
            //Invoke chain
            var updatedRate = await _winnerHandler.Handle(rates);

            foreach (var rate in updatedRate)
            {
                await _paymentService.ApportionCashByRateAsync(roomId, rate.AccountId, rate.Payout);
                await _rateService.UpdateRateByRoomIdAsync(roomId, rate);
            }
        }
        catch (GeneralException ex)
        {
            _logger.LogDebug("{Msg}", ex.Message);
        }
    }
}
