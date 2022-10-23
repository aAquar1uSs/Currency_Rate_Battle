using CurrencyRateBattleServer.Dto;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;

public class GetCurrencyStateResponse
{
    public CurrencyStateDto[] CurrencyStates { get; set; }
}
