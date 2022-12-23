using System.Text.Json;
using CurrencyRateBattleServer.Domain.Entities;
using NbuClient.Dto;

namespace NbuClient;

public class NbuApiApiClient : INbuApiClient
{
    private const string Path = "NBUStatService/v1/statdirectory/exchange?json";
    private readonly HttpClient _client;

    public NbuApiApiClient(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<CurrencyDto[]?> GetCurrencyRatesAsync(CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync(Path, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Array.Empty<CurrencyDto>();
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var currencies  = await JsonSerializer.DeserializeAsync<List<CurrencyDto>>(stream, cancellationToken: cancellationToken);
        return currencies?.ToArray();
    }
}
