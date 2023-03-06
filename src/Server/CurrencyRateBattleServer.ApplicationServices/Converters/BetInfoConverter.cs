using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class BetInfoConverter
{
    public static BetInfoDto ToDto(this Bet bet)
    {
        return new BetInfoDto
        {
            СurrencyName = bet.CurrencyName,
            BetAmount = bet.BetAmount,
            Id = bet.Id,
            IsClosed = bet.IsClosed,
            PayoutAmount = bet.PayoutAmount,
            RoomDate = bet.RoomDate,
            SetDate = bet.SetDate,
            SettleDate = bet.SettleDate,
            UserCurrencyExchange = bet.UserCurrencyExchange,
            WonCurrencyExchange = bet.WonCurrencyExchange
        };
    }
}
