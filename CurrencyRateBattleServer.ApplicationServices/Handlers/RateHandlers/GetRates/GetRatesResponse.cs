using CurrencyRateBattleServer.ApplicationServices.Dto;
using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RateHandlers.GetRates;

public class GetRatesResponse
{
    public RateDto[] Rates { get; set; }
}
