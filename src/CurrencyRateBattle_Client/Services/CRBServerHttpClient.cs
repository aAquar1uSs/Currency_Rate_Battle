namespace CRBClient.Services;
using CRBClient.Helpers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class CRBServerHttpClient
{
    private readonly WebServerOptions options;
    private readonly HttpClient httpClient;
    private readonly ILogger<CRBServerHttpClient> logger;
    public CRBServerHttpClient(IOptions<WebServerOptions> options, HttpClient httpClient,
            ILogger<CRBServerHttpClient> logger)
    {
        this.options = options.Value;
        this.httpClient = httpClient;
        this.logger = logger;

        httpClient.BaseAddress = new Uri(this.options.BaseUrl);
    }

}
