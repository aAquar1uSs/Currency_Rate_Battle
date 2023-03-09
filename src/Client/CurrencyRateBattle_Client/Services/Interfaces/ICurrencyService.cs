using CRBClient.Dto;

namespace CRBClient.Services.Interfaces;

public interface ICurrencyService
{
    Task<List<CurrencyDto>> GetCurrencyRatesAsync(CancellationToken cancellationToken);
}
