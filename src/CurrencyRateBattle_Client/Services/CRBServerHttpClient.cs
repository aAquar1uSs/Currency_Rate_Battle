namespace CRBClient.Services;
using CRBClient.Helpers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class CRBServerHttpClient
{
    private readonly WebServerOptions _options;
    private readonly HttpClient _httpClient;
    private readonly ILogger<CRBServerHttpClient> _logger;
    public CRBServerHttpClient(IOptions<WebServerOptions> options, HttpClient httpClient,
            ILogger<CRBServerHttpClient> logger)
    {
        _options = options.Value;
        _httpClient = httpClient;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(this._options.BaseUrl);
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
    {
        _logger.LogInformation($"Sending request to {requestMessage.RequestUri}...");
        var responseMessage = await _httpClient.SendAsync(requestMessage);

        return responseMessage;
    }

    public async Task<string> GetAPIResults(string subURL)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        // List data response.
        var response = _httpClient.GetAsync(subURL).Result;
        if (response.IsSuccessStatusCode)
        {
            // Parse the response body.
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
        else
        {
            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            return string.Empty;
        }
    }

}
