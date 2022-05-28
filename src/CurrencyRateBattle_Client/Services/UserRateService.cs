using CRBClient.Helpers;
using CRBClient.Models;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;
using System.Net;

namespace CRBClient.Services;

public class UserRateService : IUserRateService
{
    private readonly ICRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<UserRateService> _logger;

    public UserRateService(ICRBServerHttpClient httpClient,
        IOptions<WebServerOptions> options, ILogger<UserRateService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<List<BetViewModel>> GetUserRates()
    {
        var response = await _httpClient.GetAsync(_options.GetUserBetsURL ?? "");
        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadAsAsync<List<BetViewModel>>()
            : response.StatusCode == HttpStatusCode.Unauthorized ? throw new GeneralException() : new List<BetViewModel>();
    }


    public async Task InsertUserRateAsync(RateViewModel rateViewModel)
    {
        var response = await _httpClient.PostAsync(_options.MakeBetURL ?? "", rateViewModel);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User rate is inserted.");
            return;
        }

        var errorMsg = await response.Content.ReadAsStringAsync();
        _logger.LogError("Rate Insertion: {ErrorMsg}", errorMsg);
        if (response.StatusCode == HttpStatusCode.Conflict)
            throw new GeneralException(errorMsg);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            throw new GeneralException(errorMsg);
    }

}
