using CurrencyRateBattleServer.ApplicationServices.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyHandlers.GetCurrencyRates;

public class GetCurrencyResponse
{
    public CurrencyDto[] CurrencyStates { get; set; }
}
