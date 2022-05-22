using System.Net;
using CRBClient.Helpers;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;
using System.Globalization;

namespace CRBClient.Services;

public class CommonService : ICommonService
{
    private readonly CRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<UserService> _logger;

    public CommonService(CRBServerHttpClient httpClient,
           IOptions<WebServerOptions> options, ILogger<UserService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }


    public async Task<string> GetUserBalanceAsync()
    {
        var balance = string.Empty;
        var response = await _httpClient.GetAsync(_options.GetBalanceURL);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (decimal.TryParse(response.Content.ReadAsStringAsync().Result, out var bal))
            {
                balance = "BALANCE: " + bal.ToString("C", new CultureInfo("uk-UA"));
            }
        }
        return response.StatusCode == HttpStatusCode.Unauthorized ? throw new CustomException() : balance;
    }

}
