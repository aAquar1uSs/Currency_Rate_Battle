using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetUserBets;

public class GetUserBetsResponse
{
    public RateDto[] Bets { get; set; }
}
