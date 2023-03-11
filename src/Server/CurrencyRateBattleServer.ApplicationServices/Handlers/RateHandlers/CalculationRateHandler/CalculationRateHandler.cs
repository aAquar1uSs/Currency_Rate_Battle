using CurrencyRateBattleServer.ApplicationServices.Handlers.HistoryHandlers.CreateAccountHistory;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.CalculationRateHandler;

public class CalculationRateHandler : IRequestHandler<CalculationRateCommand>
{
    private readonly ILogger<CalculationRateHandler> _logger;
    private readonly IRateQueryRepository _rateQueryRepository;
    private readonly IRateRepository _rateRepository;
    private readonly IRoomQueryRepository _roomQueryRepository;
    private readonly ICurrencyQueryRepository _currencyQueryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMediator _mediator;

    public CalculationRateHandler(ILogger<CalculationRateHandler> logger, IRateQueryRepository rateQueryRepository,
        IRoomQueryRepository roomQueryRepository, ICurrencyQueryRepository currencyQueryRepository, IAccountRepository accountRepository,
        IMediator mediator, IRateRepository rateRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rateQueryRepository = rateQueryRepository ?? throw new ArgumentNullException(nameof(rateQueryRepository));
        _roomQueryRepository = roomQueryRepository ?? throw new ArgumentNullException(nameof(roomQueryRepository));
        _currencyQueryRepository = currencyQueryRepository ?? throw new ArgumentNullException(nameof(currencyQueryRepository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
    }

    public async Task<Unit> Handle(CalculationRateCommand request, CancellationToken cancellationToken)
    {
        var closedRooms = await _roomQueryRepository.RoomClosureCheckAsync(cancellationToken);
        var roomIds = closedRooms.Select(x => x.Id).ToArray();

        var rates = await _rateQueryRepository.GetActiveRateByRoomIdsAsync(roomIds, cancellationToken);

        if (rates.Length == 0)
            return Unit.Value;

        var calculateRates = await Calculate(rates, cancellationToken);

        await _rateRepository.UpdateRange(calculateRates, cancellationToken);

        foreach (var rate in calculateRates)
        {
            var account = await _accountRepository.Get(rate.AccountId, cancellationToken);
            if (account is null)
            {
                _logger.LogWarning("Account not found when rate was processed. Skip processing for account id {Id}", rate.AccountId.Value);
                continue;
            }

            var moneyCreatedResult = Amount.TryCreate(rate.Payout?.Value);
            if (moneyCreatedResult.IsFailure)
            {
                _logger.LogWarning("Failed to payment process. For account id {id}. Skip processing. {Reason}", account.Id.Value, moneyCreatedResult.Error);
                continue;
            }

            account.ApportionCash(moneyCreatedResult.Value);

            await _accountRepository.Update(account, cancellationToken);
            
            var command = new CreateHistoryCommand(account.UserEmail.Value, rate.RoomId.Id, DateTime.UtcNow, moneyCreatedResult.Value.Value, true);
            _ = await _mediator.Send(command, cancellationToken);
        }

        return Unit.Value;
    }

    private async Task<Rate[]> Calculate(Rate[] rates, CancellationToken cancellationToken)
    {
        if (rates.Length == 1)
        {
            var rate = rates.First();
            rate.Change(true, rate.Amount.Value, DateTime.UtcNow);
            return rates;
        }

        foreach (var rate in rates)
        {
            var currencyRate = await _currencyQueryRepository.GetRateByCurrencyName(rate.CurrencyName.Value, cancellationToken);
            if (currencyRate != null && rate.RateCurrencyExchange.Value == Math.Round(currencyRate, 2))
                rate.IsWonBet();
            else
            {
                rate.IsLoseBet();
            }
        }

        return CalculateWinningRates(rates.ToArray());
    }

    private Rate[] CalculateWinningRates(Rate[] rates)
    {
        var commonBank = rates.Sum(rate => rate.Amount.Value);

        var winnerCount = rates.Count(rate => rate.IsWon);

        if (winnerCount == 0)
        {
            foreach (var rate in rates)
            {
                rate.CreatePayout(0m, DateTime.UtcNow);
            }
        }
        else if (winnerCount == 1)
        {
            var rate = rates.FirstOrDefault(rate => rate.IsWon);
            if (rate != null)
            {
                var payout = rate.Amount.Value + (0.5m * (commonBank - rate.Amount.Value));
                rate.CreatePayout(payout, DateTime.UtcNow);
            }
        }
        else
        {
            if (CheckSameRates(rates))
                UnusualCalculation(ref rates, commonBank);
            else
                StandartCalculation(ref rates, commonBank);
        }

        return rates;
    }

    private static void StandartCalculation(ref Rate[] rates, decimal commonBank)
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
    }

    private static void UnusualCalculation(ref Rate[] rates, decimal commonBank)
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
    }

    private static bool CheckSameRates(Rate[] rates)
    {
        var winnings = rates.Where(rate => rate.IsWon).ToArray();
        var amount = winnings.First().Amount;

        return winnings.All(rate => rate.Amount == amount);
    }
}
