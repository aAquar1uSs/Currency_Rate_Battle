using CRBClient.Dto;

namespace CRBClient.Services.Interfaces;

public interface ICurrencyStateService
{
    Task<List<CurrencyDto>> GetCurrencyRatesAsync(CancellationToken cancellationToken);
}
