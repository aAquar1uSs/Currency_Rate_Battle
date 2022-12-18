﻿using CSharpFunctionalExtensions;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;

namespace CurrencyRateBattleServer.Domain.Entities;

public class Rate
{
    public OneId Id { get; }

    public DateTime SetDate { get; }

    public RateCurrencyExchange RateCurrencyExchange { get; }

    public Amount Amount { get; }

    //Date when the rate is settled
    public DateTime? SettleDate { get; }

    public Payout? Payout { get; private set; }

    public bool IsClosed { get; private set; }

    public bool IsWon { get; }

    public RoomId RoomId { get; }

    public CurrencyCode CurrencyCode { get; }

    public AccountId AccountId { get; }

    public Rate(OneId id,
        DateTime setDate,
        RateCurrencyExchange rateCurrencyExchange,
        Amount amount,
        DateTime? settleDate,
        Payout? payout,
        bool isClosed,
        bool isWon,
        RoomId roomId,
        CurrencyCode currencyCode,
        AccountId accountId)
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
        CurrencyCode = currencyCode;
        AccountId = accountId;
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
        string currencyCode,
        Guid accountId)
    {
        var oneIdResult = OneId.TryCreate(id);
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

        var currencyCodeResult = CurrencyCode.TryCreate(currencyCode);
        if (currencyCodeResult.IsFailure)
            return Result.Failure<Rate>(currencyCodeResult.Error);

        var accountOneIdResult = AccountId.TryCreate(accountId);
        if (accountOneIdResult.IsFailure)
            return Result.Failure<Rate>(accountOneIdResult.Error);

        if (payout is null)
            return new Rate(oneIdResult.Value, setDate, rateCurrencyExchangeResult.Value, amountResult.Value,
                settleDate, null, isClosed, isWon, roomOneIdResult.Value, currencyCodeResult.Value,
                accountOneIdResult.Value);

        var payoutResult = Payout.TryCreate((decimal)payout);
        if (payoutResult.IsFailure)
            return Result.Failure<Rate>(payoutResult.Error);

        return new Rate(oneIdResult.Value, setDate, rateCurrencyExchangeResult.Value, amountResult.Value,
            settleDate, payoutResult.Value, isClosed, isWon, roomOneIdResult.Value, currencyCodeResult.Value,
            accountOneIdResult.Value);
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
        Guid accountId)
    {
        var oneId = OneId.Create(id);
        var rateExchange = RateCurrencyExchange.Create(rateCurrencyExchange);
        var amountDomain = Amount.Create(amount);

        var roomOneId = RoomId.Create(roomId);
        var currencyCode = CurrencyCode.Create(currency);

        var accountOneId = AccountId.Create(accountId);
        if (payout is null)
            return new Rate(oneId, setDate, rateExchange, amountDomain,
                settleDate, null, isClosed, isWon, roomOneId, currencyCode,
                accountOneId);

        var payoutDomain = Payout.Create((decimal)payout);

        return new Rate(oneId, setDate, rateExchange, amountDomain,
            settleDate, payoutDomain, isClosed, isWon, roomOneId, currencyCode,
            accountOneId);
    }

    public void Change(bool isClosed, 
        decimal payout)
    {
        IsClosed = isClosed;
        Payout = Payout.Create(payout);
    }
    
    public Result Change(decimal payout,
        bool isClosed)
    {
        var payoutResult = Payout.TryCreate(payout);
        if (payoutResult.IsFailure)
            return Result.Failure(payoutResult.Error);

        IsClosed = isClosed;
        return Result.Success();
    }
}
