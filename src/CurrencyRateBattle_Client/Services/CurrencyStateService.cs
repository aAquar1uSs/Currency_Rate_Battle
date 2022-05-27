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

    public async Task<List<CurrencyStateDto>> GetCurrencyRatesAsync()
    {
        var response = await _httpClient.GetAsync(_options.GetCurrencyRatesURL ?? "");

        List<CurrencyStateDto>? currencyStates = null;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await  response.Content.ReadAsStringAsync();
            currencyStates = JsonSerializer.Deserialize<List<CurrencyStateDto>>(content);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new GeneralException();

        if (currencyStates is null)
            currencyStates = new List<CurrencyStateDto>();

        return currencyStates;
    }
}
