using NbuClient.Dto;

namespace NbuClient;

public interface INbuApiClient
{
    Task<CurrencyDto[]?> GetCurrencyRatesAsync(CancellationToken cancellationToken);
}
