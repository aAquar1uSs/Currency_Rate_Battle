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
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User rates are loaded successfully");
            return await response.Content.ReadAsAsync<List<BetViewModel>>();
        }
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("User rates not loaded, user is unauthorized");
            throw new GeneralException();
        }
        return new List<BetViewModel>();
    }


    public async Task InsertUserRateAsync(RateViewModel rateViewModel)
    {
        var response = await _httpClient.PostAsync(_options.MakeBetURL ?? "", rateViewModel);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User rate are successfully added");
            return;
        }

        var errorMsg = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            _logger.LogInformation(errorMsg);
            throw new GeneralException(errorMsg);
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            _logger.LogInformation(errorMsg);
            throw new GeneralException(errorMsg);
        }
    }

}
