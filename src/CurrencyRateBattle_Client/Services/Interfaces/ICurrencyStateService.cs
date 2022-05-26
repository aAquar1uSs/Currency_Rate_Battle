using CRBClient.Dto;

namespace CRBClient.Services.Interfaces;

public interface ICurrencyStateService
{
    Task<List<CurrencyStateDto>> GetCurrencyRatesAsync();
}
