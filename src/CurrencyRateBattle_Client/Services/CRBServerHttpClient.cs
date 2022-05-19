using System.Net.Http.Formatting;
using CRBClient.Helpers;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using CRBClient.Models;

namespace CRBClient.Services;

public class CRBServerHttpClient
{
    private readonly WebServerOptions _options;
    private readonly HttpClient _httpClient;
    private readonly ILogger<CRBServerHttpClient> _logger;

    public CRBServerHttpClient(IOptions<WebServerOptions> options,
            ILogger<CRBServerHttpClient> logger)
    {
        _options = options.Value;
       // _httpClient = httpClient;
        _logger = logger;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_options.BaseUrl)
        };
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
    {
        _logger.LogInformation($"Sending request to {requestMessage.RequestUri}...");
        var responseMessage = await _httpClient.SendAsync(requestMessage);

        return responseMessage;
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string requestUrl, T content)
    {
        _logger.LogInformation($"Sending request to {requestUrl}...");
        var response = await _httpClient.PostAsync(requestUrl, content, new JsonMediaTypeFormatter());
        return response;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUrl)
    {
        _logger.LogInformation($"Sending request to {requestUrl}...");
        var response = await _httpClient.GetAsync(requestUrl);
        return response;
    }

    public void SetTokenInHeader(string token)
    {
        ClearHeader();

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearHeader()
    {
        _httpClient.DefaultRequestHeaders.Clear();
    }

    public async Task<string> GetAPIResultsAsync(string subURL)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        // List data response.
        var response = await _httpClient.GetAsync(subURL);
        if (response.IsSuccessStatusCode)
        {
            // Parse the response body.
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        return string.Empty;
    }

}
