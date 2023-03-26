using System.Net.Http.Formatting;
using CRBClient.Helpers;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using CRBClient.Services.Interfaces;
using Uri = System.Uri;

namespace CRBClient.Services;

public class CRBServerClient : ICRBServerHttpClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CRBServerClient> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private ISession? Session => _httpContextAccessor.HttpContext?.Session;

    public CRBServerClient(IOptions<WebServerOptions> options,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CRBServerClient> logger)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _httpClient = new HttpClient { BaseAddress = new Uri(options.Value.BaseUrl ?? "") };
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
    {
        _logger.LogInformation("Sending request to {RequestMessage}...", requestMessage.RequestUri);

        if (Session is not null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.GetString("token"));
        }

        var responseMessage = await _httpClient.SendAsync(requestMessage);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string requestUrl, T content, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending request to {RequestUrl}...", requestUrl);

        if (Session is not null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.GetString("token"));
        }

        var response = await _httpClient.PostAsync(requestUrl, content, new JsonMediaTypeFormatter(), CancellationToken.None);
        return response;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending request to {RequestUrl}...", requestUrl);

        if (Session is not null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.GetString("token"));
        }

        var response = await _httpClient.GetAsync(requestUrl);
        return response;
    }

    public void ClearHeader()
    {
        _httpClient.DefaultRequestHeaders.Clear();
    }

    public void Dispose()
    {
        if (_httpClient != null)
        {
            _httpClient.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}
