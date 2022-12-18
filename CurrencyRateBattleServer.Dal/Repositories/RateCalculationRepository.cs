﻿using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

public class RateCalculationRepository : IRateCalculationRepository
{
    private readonly ILogger<RateCalculationRepository> _logger;
    private readonly IRateRepository _rateRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly WinnerHandler _winnerHandler;
    private readonly CalculationHandler _calculationHandler;

    public RateCalculationRepository(ILogger<RateCalculationRepository> logger,
        IRateRepository rateRepository,
        IPaymentRepository paymentRepository)
    {
        _logger = logger;
        _rateRepository = rateRepository;
        _paymentRepository = paymentRepository;

        //Chain of responsibility
        _winnerHandler = new WinnerHandler();
        _calculationHandler = new CalculationHandler();

        _ = _winnerHandler.SetNext(_calculationHandler);
    }

    public async Task StartRateCalculationByRoomIdAsync(Guid roomId)
    {
        _logger.LogInformation($"{nameof(StartRateCalculationByRoomIdAsync)} was caused.");
        var rates = await _rateRepository.GetRateByRoomIdAsync(roomId);

        if (rates.Any(r => r.IsClosed))
            return;

        //Invoke chain
        var updatedRate = await _winnerHandler.Handle(rates);

        foreach (var rate in updatedRate)
        {
            await _paymentRepository.ApportionCashByRateAsync(roomId, rate.AccountId, rate.Payout);
            await _rateRepository.UpdateRateByRoomIdAsync(roomId, rate);
        }
    }
}
