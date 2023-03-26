using System.Net;
using CRBClient.Dto;
using CRBClient.Helpers;
using CRBClient.Services.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Uri = CRBClient.Helpers.Uri;

namespace CRBClient.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ILogger<CurrencyService> _logger;
    private readonly ICRBServerHttpClient _httpClient;
    private readonly Uri _uri;

    public CurrencyService(ILogger<CurrencyService> logger, ICRBServerHttpClient httpClient, IOptions<Uri> uriOption)
    {
        _logger = logger;
        _httpClient = httpClient;
        _uri = uriOption.Value;
    }

    public async Task<List<CurrencyDto>> GetCurrencyRatesAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_uri.GetCurrencyRatesURL, cancellationToken);
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("Currency rate are loaded successfully");
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<List<CurrencyDto>>(content) ?? new List<CurrencyDto>();
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("Currency rate not loaded, user is unauthorized");
            throw new GeneralException();
        }

        return new List<CurrencyDto>();
    }
}
