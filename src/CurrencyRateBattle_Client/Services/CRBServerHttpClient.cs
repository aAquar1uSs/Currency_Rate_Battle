using System.Net.Http.Formatting;
using CRBClient.Helpers;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using CRBClient.Services.Interfaces;

namespace CRBClient.Services;

public class CRBServerHttpClient : ICRBServerHttpClient
{
    private readonly WebServerOptions _options;
    private readonly HttpClient _httpClient;
    private readonly ILogger<CRBServerHttpClient> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private ISession Session => _httpContextAccessor.HttpContext.Session;

    public CRBServerHttpClient(IOptions<WebServerOptions> options,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CRBServerHttpClient> logger)
    {
        _options = options.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _httpClient = new HttpClient { BaseAddress = new Uri(_options.BaseUrl ?? "") };
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
    {
        _logger.LogInformation($"Sending request to {requestMessage.RequestUri}...");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Session.GetString("token"));

        var responseMessage = await _httpClient.SendAsync(requestMessage);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string requestUrl, T content)
    {
        _logger.LogInformation($"Sending request to {requestUrl}...");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Session.GetString("token"));

        var response = await _httpClient.PostAsync(requestUrl, content, new JsonMediaTypeFormatter());
        return response;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUrl)
    {
        _logger.LogInformation($"Sending request to {requestUrl}...");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Session.GetString("token"));

        var response = await _httpClient.GetAsync(requestUrl);
        return response;
    }

    public void ClearHeader()
    {
        _httpClient.DefaultRequestHeaders.Clear();
    }
}
