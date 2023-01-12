using System.Net;
using CRBClient.Dto;
using CRBClient.Helpers;
using CRBClient.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CRBClient.Services;

public class CurrencyStateService : ICurrencyStateService
{
    private readonly ILogger<CurrencyStateService> _logger;

    private readonly ICRBServerHttpClient _httpClient;

    private readonly WebServerOptions _options;

    public CurrencyStateService(ILogger<CurrencyStateService> logger,
        ICRBServerHttpClient httpClient,
        IOptions<WebServerOptions> options)
    {
        _logger = logger;
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<List<CurrencyDto>> GetCurrencyRatesAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_options.GetCurrencyRatesURL ?? "", cancellationToken);

        List<CurrencyDto>? currencyStates = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("Currency rate are loaded successfully");
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            currencyStates = JsonSerializer.Deserialize<List<CurrencyDto>>(content);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("Currency rate not loaded, user is unauthorized");
            throw new GeneralException();
        }

        if (currencyStates is null)
            currencyStates = new List<CurrencyDto>();

        return currencyStates;
    }
}
