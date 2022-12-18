using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.CalculationRateHandler;

public class CalculationRateHandler : IRequestHandler<CalculationRateCommand>
{
    private readonly ILogger<CalculationRateHandler> _logger;
    private readonly IRateRepository _rateRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ICurrencyStateRepository _currencyStateRepository;

    public CalculationRateHandler(ILogger<CalculationRateHandler> logger,
        IRateRepository rateRepository, IRoomRepository roomRepository,
        ICurrencyStateRepository currencyStateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _currencyStateRepository = currencyStateRepository ?? throw new ArgumentNullException(nameof(currencyStateRepository));
    }
        
    public async Task<Unit> Handle(CalculationRateCommand request, CancellationToken cancellationToken)
    {
        var closedRooms = await _roomRepository.RoomClosureCheckAsync(cancellationToken);
        var roomIds = closedRooms.Select(x => x.Id).ToArray();

        var rates = await _rateRepository.GetRateByRoomIdAsync(roomIds, cancellationToken);
        
        if (rates.Any(r => r.IsClosed))
            return Unit.Value;

        var winnerRates = await GetWinnerRates(rates, cancellationToken);
        
    }

    private async Task<Rate[]> GetWinnerRates(Rate[] rates, CancellationToken cancellationToken)
    {
        if (rates.Length == 1)
        {
            var rate = rates.First();
            rate.Change(true, rate.Amount.Value);
            return rates;
        }

        foreach (var rate in rates)
        {
            var currencyState = await _currencyStateRepository.GetCurrencyStateByRoomIdAsync(rate.RoomId, cancellationToken);
            
        }
    }
}
