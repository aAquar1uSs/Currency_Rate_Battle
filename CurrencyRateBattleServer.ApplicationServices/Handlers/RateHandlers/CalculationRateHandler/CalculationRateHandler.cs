using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.CalculationRateHandler;

public class CalculationRateHandler : IRequestHandler<CalculationRateCommand>
{
    private readonly ILogger<CalculationRateHandler> _logger;
    private readonly IRateRepository _rateRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ICurrencyStateRepository _currencyStateRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountHistoryRepository _accountHistoryRepository;

    public CalculationRateHandler(ILogger<CalculationRateHandler> logger,
        IRateRepository rateRepository, IRoomRepository roomRepository,
        ICurrencyStateRepository currencyStateRepository,
        IAccountRepository accountRepository,
        IAccountHistoryRepository accountHistoryRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _currencyStateRepository = currencyStateRepository ?? throw new ArgumentNullException(nameof(currencyStateRepository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _accountHistoryRepository = accountHistoryRepository ?? throw new ArgumentNullException(nameof(accountHistoryRepository));
    }

    public async Task<Unit> Handle(CalculationRateCommand request, CancellationToken cancellationToken)
    {
        var closedRooms = await _roomRepository.RoomClosureCheckAsync(cancellationToken);
        var roomIds = closedRooms.Select(x => x.Id).ToArray();

        var rates = await _rateRepository.GetRateByRoomIdsAsync(roomIds, cancellationToken);

        if (rates.Any(r => r.IsClosed))
            return Unit.Value;

        var calculateRates = await Calculate(rates, cancellationToken);

        await _rateRepository.UpdateRateByRoomIdAsync(calculateRates, cancellationToken);

        foreach (var rate in calculateRates)
        {
            var account = await _accountRepository.GetAsync(rate.AccountId, cancellationToken);
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

            await _accountRepository.UpdateAsync(account, cancellationToken);

            var accountHistoryId = OneId.GenerateId();
            var accountHistory = AccountHistory.Create(accountHistoryId.Id, account.Id.Id,
                DateTime.UtcNow, moneyCreatedResult.Value.Value, true, rate.RoomId.Id);
            await _accountHistoryRepository.CreateAsync(accountHistory, cancellationToken);
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
            var currencyState = await _currencyStateRepository.GetCurrencyStateByRoomIdAsync(rate.RoomId, cancellationToken);
            if (currencyState != null && rate.RateCurrencyExchange == currencyState.CurrencyExchangeRate)
            {
                rate.IsWonBet();
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
