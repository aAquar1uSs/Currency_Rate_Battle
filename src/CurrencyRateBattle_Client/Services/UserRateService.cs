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

    public string DeleteUserRate()
    {
        throw new NotImplementedException();
    }

    public async Task<List<BetViewModel>> GetUserRates()
    {
        var response = await _httpClient.GetAsync(_options.GetUserBetsURL ?? "");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsAsync<List<BetViewModel>>();
        }

        return response.StatusCode == HttpStatusCode.Unauthorized ? throw new CustomException() : new List<BetViewModel>();
    }


    public async Task InsertUserRateAsync(RateViewModel rateViewModel)
    {
        var response = await _httpClient.PostAsync(_options.MakeBetURL, rateViewModel);

        if (response.StatusCode == HttpStatusCode.OK)
            return;

        var errorMsg = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.Conflict)
            throw new CustomException(errorMsg);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            throw new CustomException(errorMsg);
    }

    public string UpdateUserRate()
    {
        throw new NotImplementedException();
    }
}
