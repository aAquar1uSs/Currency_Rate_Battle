using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;

public class GetUserBetsResponse
{
    public BetInfoDto[] Bets { get; set; }
}
