using CurrencyRateBattleServer.ApplicationServices.Handlers.PayoutHandler;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using MediatR;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.CalculationRateHandler;

public class CalculationRateHandler : IRequestHandler<CalculationRateCommand>
{
    private readonly IRateQueryRepository _rateQueryRepository;
    private readonly IRateRepository _rateRepository;
    private readonly IRoomQueryRepository _roomQueryRepository;
    private readonly ICurrencyQueryRepository _currencyQueryRepository;
    private readonly IMediator _mediator;

    public CalculationRateHandler(IRateQueryRepository rateQueryRepository, IRoomQueryRepository roomQueryRepository, 
        ICurrencyQueryRepository currencyQueryRepository, IRateRepository rateRepository, IMediator mediator)
    {
        _rateQueryRepository = rateQueryRepository ?? throw new ArgumentNullException(nameof(rateQueryRepository));
        _roomQueryRepository = roomQueryRepository ?? throw new ArgumentNullException(nameof(roomQueryRepository));
        _currencyQueryRepository = currencyQueryRepository ?? throw new ArgumentNullException(nameof(currencyQueryRepository));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<Unit> Handle(CalculationRateCommand request, CancellationToken cancellationToken)
    {
        var closedRooms = await _roomQueryRepository.RoomClosureCheckAsync(cancellationToken);
        var roomIds = closedRooms.Select(x => x.Id).ToArray();

        var rates = await _rateQueryRepository.GetActiveRateByRoomIdsAsync(roomIds, cancellationToken);

        if (rates.Length == 0)
            return Unit.Value;

        await Process(rates, cancellationToken);

        return Unit.Value;
    }

    private async Task Process(Rate[] rates, CancellationToken cancellationToken)
    {
        if (rates.Length == 1)
            await TechnicalReturn(rates.First(), cancellationToken);
        
        foreach (var rate in rates)
        {
            var currencyRate = await _currencyQueryRepository.GetRateByCurrencyName(rate.CurrencyName.Value, cancellationToken);
            if (rate.RateCurrencyExchange.Value == Math.Round(currencyRate, 2))
                rate.IsWonBet(currencyRate);
            else
            {
                rate.IsLoseBet(currencyRate);
            }
        }

        await CalculateWinningRates(rates, cancellationToken);
    }

    private async Task TechnicalReturn(Rate rate, CancellationToken cancellationToken)
    {
        var currencyRate = await _currencyQueryRepository.GetRateByCurrencyName(rate.CurrencyName.Value, cancellationToken);
        rate.Change(true, rate.Amount.Value, DateTime.UtcNow, currencyRate);
        
        await _rateRepository.Update(rate, cancellationToken);
    }

    private async Task CalculateWinningRates(Rate[] rates, CancellationToken cancellationToken)
    {
        var commonBank = rates.Sum(rate => rate.Amount.Value);
        var winnerCount = rates.Count(rate => rate.IsWon);

        switch (winnerCount)
        {
            case 0:
            {
                await CreateEmptyPayout(rates, cancellationToken);
                return;
            }
            case 1:
            {
                await CreatePayoutForOneWinner(rates.First(x => x.IsWon), commonBank, cancellationToken);
                return;
            }
            default:
            {
                if (CheckSameRates(rates))
                    await UnusualCalculation(rates, commonBank, cancellationToken);
                else
                    await StandartCalculation(rates, commonBank, cancellationToken);
                return;
            }
        }
    }

    private async Task CreateEmptyPayout(Rate[] rates, CancellationToken cancellationToken)
    {
        foreach (var rate in rates)
        {
            rate.CreatePayout(0m, DateTime.UtcNow);
        }
        
        await _rateRepository.UpdateRange(rates, cancellationToken);
        _ = rates.Select(async x =>
        {
            await MakePayout(x.AccountId.Id, 0m, x.RoomId.Id, cancellationToken);
        });
    }

    private async Task CreatePayoutForOneWinner(Rate rate, decimal commonBank, CancellationToken cancellationToken)
    {
        var payout = rate.Amount.Value + (0.5m * (commonBank - rate.Amount.Value));
        rate.CreatePayout(payout, DateTime.UtcNow);

        await _rateRepository.Update(rate, cancellationToken);

        await MakePayout(rate.AccountId.Id, payout, rate.RoomId.Id, cancellationToken);
    }

    private async Task UnusualCalculation(Rate[] rates, decimal commonBank, CancellationToken cancellationToken)
    {
        rates.ToList().Sort((x, y) => x.SetDate.CompareTo(y.SetDate));

        var winners = rates.Where(rate => rate.IsWon).ToArray();
        var winnerCount = winners.Length;
        var rateAmount = winners.First().Amount.Value;

        var loserBank = commonBank - (rateAmount * winnerCount);
        var step = 2 * loserBank / (winnerCount * (winnerCount - 1));

        var winningMoney = Enumerable
            .Repeat(0m, winnerCount)
            .Select((x, index) => (index * step) + rateAmount).ToList();

        var index = winnerCount - 1;
        rates.ToList().ForEach(rate =>
        {
            var payout = rate.IsWon ? Math.Round(winningMoney[index--], 2) : 0m;
            rate.CreatePayout(payout, DateTime.UtcNow);
        });
        
        await _rateRepository.UpdateRange(rates, cancellationToken);

        _ = rates.Select(async x =>
        {
            await MakePayout(x.AccountId.Id, x.Payout.Value, x.RoomId.Id, cancellationToken);
        });
    }
    
    private async Task StandartCalculation(Rate[] rates, decimal commonBank, CancellationToken cancellationToken)
    {
        var winnerBank = rates
            .Where(rate => rate.IsWon)
            .Sum(rate => rate.Amount.Value);

        var kef = commonBank / winnerBank;

        foreach (var rate in rates)
        {
            var payout = rate.IsWon ? Math.Round(rate.Amount.Value * kef, 2) : 0;
            rate.CreatePayout(payout, DateTime.UtcNow);
        }

        await _rateRepository.UpdateRange(rates, cancellationToken);
        
        _ = rates.Select(async x =>
        {
            await MakePayout(x.AccountId.Id, x.Payout.Value, x.RoomId.Id, cancellationToken);
        });
    }

    private static bool CheckSameRates(Rate[] rates)
    {
        var winnings = rates.Where(rate => rate.IsWon).ToArray();
        var amount = winnings.First().Amount;

        return winnings.All(rate => rate.Amount == amount);
    }

    private async Task MakePayout(Guid accountId, decimal payout, Guid roomId, CancellationToken cancellationToken)
    {
        var command = new PayoutCommand(accountId, payout, roomId);
        _ = await _mediator.Send(command, cancellationToken);
    }
}
