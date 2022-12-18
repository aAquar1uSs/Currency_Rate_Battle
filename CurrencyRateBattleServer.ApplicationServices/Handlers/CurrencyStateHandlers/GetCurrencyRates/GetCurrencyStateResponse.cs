using CurrencyRateBattleServer.ApplicationServices.Dto;
namespace CurrencyRateBattleServer.ApplicationServices.Handlers.CurrencyStateHandlers.GetCurrencyRates;

public class GetCurrencyStateResponse
{
    public CurrencyDto[] CurrencyStates { get; set; }
}
