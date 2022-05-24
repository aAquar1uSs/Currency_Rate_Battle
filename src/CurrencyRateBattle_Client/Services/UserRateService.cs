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
        var response = await _httpClient.GetAsync(_options.GetUserBetsURL);
        //var response = await _httpClient.GetAsync("api/rates/get-user-bets");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsAsync<List<BetViewModel>>();
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new CustomException();
        }

        return new List<BetViewModel>();
    }


    public string InsertUserRate()
    {
        throw new NotImplementedException();
    }

    public string UpdateUserRate()
    {
        throw new NotImplementedException();
    }
}
