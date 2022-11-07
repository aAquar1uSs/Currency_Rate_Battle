using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Domain.Entities;

namespace CurrencyRateBattleServer.ApplicationServices.Converters;

public static class BetInfoConverter
{
    public static BetInfoDto ToDto(this BetInfo betInfo)
    {
        return new BetInfoDto
        {
            СurrencyName = betInfo.СurrencyName,
            BetAmount = betInfo.BetAmount,
            Id = betInfo.Id,
            IsClosed = betInfo.IsClosed,
            PayoutAmount = betInfo.PayoutAmount,
            RoomDate = betInfo.RoomDate,
            SetDate = betInfo.SetDate,
            SettleDate = betInfo.SettleDate,
            UserCurrencyExchange = betInfo.UserCurrencyExchange,
            WonCurrencyExchange = betInfo.WonCurrencyExchange
        };
    }

    public static BetInfoDto[] ToDto(this BetInfo[] betInfos)
    {
        return betInfos.Select(x => x.ToDto()).ToArray();
    }
}
