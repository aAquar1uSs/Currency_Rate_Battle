using CurrencyRateBattleServer.ApplicationServices.Dto;
namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;

public class GetCurrencyResponse
{
    public CurrencyDto[] CurrencyStates { get; set; }
}
