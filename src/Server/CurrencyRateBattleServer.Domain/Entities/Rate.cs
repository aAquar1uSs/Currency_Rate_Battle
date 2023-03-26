using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class Rate
{
    public CustomId Id { get; }

    public DateTime SetDate { get; }

    public RateCurrencyExchange RateCurrencyExchange { get; }

    public Amount Amount { get; }
    
    public DateTime? SettleDate { get; private set; }

    public Payout? Payout { get; private set; }

    public bool IsClosed { get; private set; }

    public bool IsWon { get; private set; }
    
    public Amount? RealCurrencyExchange { get; set; }

    public RoomId RoomId { get; }

    public CurrencyName CurrencyName { get; }

    public AccountId AccountId { get; }

    public Rate(CustomId id,
        DateTime setDate,
        RateCurrencyExchange rateCurrencyExchange,
        Amount amount,
        DateTime? settleDate,
        Payout? payout,
        bool isClosed,
        bool isWon,
        RoomId roomId,
        CurrencyName currencyCode,
        AccountId accountId,
        Amount? realCurrencyExchange = null)
    {
        Id = id;
        RateCurrencyExchange = rateCurrencyExchange;
        Amount = amount;
        SetDate = setDate;
        SettleDate = settleDate;
        Payout = payout;
        IsClosed = isClosed;
        IsWon = isWon;
        RoomId = roomId;
        CurrencyName = currencyCode;
        AccountId = accountId;
        RealCurrencyExchange = realCurrencyExchange;
    }

    public static Result<Rate> TryCreate(Guid id,
        DateTime setDate,
        decimal rateCurrencyExchange,
        decimal amount,
        DateTime? settleDate,
        decimal? payout,
        bool isClosed,
        bool isWon,
        Guid roomId,
        string currencyName,
        Guid accountId,
        decimal? realCurrencyExchange = null)
    {
        var oneIdResult = CustomId.TryCreate(id);
        if (oneIdResult.IsFailure)
            return Result.Failure<Rate>(oneIdResult.Error);

        var rateCurrencyExchangeResult = RateCurrencyExchange.TryCreate(rateCurrencyExchange);
        if (rateCurrencyExchangeResult.IsFailure)
            return Result.Failure<Rate>(rateCurrencyExchangeResult.Error);

        var amountResult = Amount.TryCreate(amount);
        if (amountResult.IsFailure)
            return Result.Failure<Rate>(amountResult.Error);

        var roomOneIdResult = RoomId.TryCreate(roomId);
        if (roomOneIdResult.IsFailure)
            return Result.Failure<Rate>(roomOneIdResult.Error);

        var currencyNameResult = CurrencyName.TryCreate(currencyName);
        if (currencyNameResult.IsFailure)
            return Result.Failure<Rate>(currencyNameResult.Error);

        var accountOneIdResult = AccountId.TryCreate(accountId);
        if (accountOneIdResult.IsFailure)
            return Result.Failure<Rate>(accountOneIdResult.Error);
        
        var realExchangeRateResult = realCurrencyExchange is null ? null : Amount.TryCreate(realCurrencyExchange);
        if (realCurrencyExchange is not null && realExchangeRateResult.IsFailure)
            return Result.Failure<Rate>(amountResult.Error);

        if (payout is null)
            return new Rate(oneIdResult.Value, setDate, rateCurrencyExchangeResult.Value, amountResult.Value,
                settleDate, null, isClosed, isWon, roomOneIdResult.Value, currencyNameResult.Value,
                accountOneIdResult.Value, realExchangeRateResult.Value);

        var payoutResult = Payout.TryCreate((decimal)payout);
        if (payoutResult.IsFailure)
            return Result.Failure<Rate>(payoutResult.Error);

        return new Rate(oneIdResult.Value, setDate, rateCurrencyExchangeResult.Value, amountResult.Value,
            settleDate, payoutResult.Value, isClosed, isWon, roomOneIdResult.Value, currencyNameResult.Value,
            accountOneIdResult.Value, realExchangeRateResult.Value);
    }

    public static Rate Create(Guid id,
        DateTime setDate,
        decimal rateCurrencyExchange,
        decimal amount,
        DateTime? settleDate,
        decimal? payout,
        bool isClosed,
        bool isWon,
        Guid roomId,
        string currency,
        Guid accountId,
        decimal? realCurrencyExchange = null)
    {
        var oneId = CustomId.Create(id);
        var rateExchange = RateCurrencyExchange.Create(rateCurrencyExchange);
        var amountDomain = Amount.Create(amount);

        var roomOneId = RoomId.Create(roomId);
        var currencyName = CurrencyName.Create(currency);

        var accountOneId = AccountId.Create(accountId);

        var realExchangeRate = realCurrencyExchange is null ? null : Amount.Create(realCurrencyExchange.Value);
        
        if (payout is null)
            return new Rate(oneId, setDate, rateExchange, amountDomain,
                settleDate, null, isClosed, isWon, roomOneId, currencyName,
                accountOneId, realExchangeRate);

        var payoutDomain = Payout.Create((decimal)payout);

        return new Rate(oneId, setDate, rateExchange, amountDomain,
            settleDate, payoutDomain, isClosed, isWon, roomOneId, currencyName,
            accountOneId, realExchangeRate);
    }

    public void Update(bool isClosed,
        decimal payout,
        DateTime settledDate,
        decimal realCurrencyRate)
    {
        IsClosed = isClosed;
        Payout = Payout.Create(payout);
        SettleDate = settledDate;
        RealCurrencyExchange = Amount.Create(realCurrencyRate);
    }

    public void IsWonBet(decimal realCurrencyRate)
    {
        IsWon = true;
        IsClosed = true;
        RealCurrencyExchange = Amount.Create(realCurrencyRate);
    }

    public void IsLoseBet(decimal realCurrencyRate)
    {
        IsWon = false;
        IsClosed = true;
        RealCurrencyExchange = Amount.Create(realCurrencyRate);
    }

    public void CreatePayout(decimal payout, DateTime settledDate)
    {
        Payout = Payout.Create(payout);
        SettleDate = settledDate;
    }
}
