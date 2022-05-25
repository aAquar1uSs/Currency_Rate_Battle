using CRBClient.Helpers;
using CRBClient.Models;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;
using System.Net;

namespace CRBClient.Services;

public class RatingService : IRatingService
{
    private readonly ICRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<UserRateService> _logger;

    public RatingService(ICRBServerHttpClient httpClient,
        IOptions<WebServerOptions> options, ILogger<UserRateService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<List<RatingViewModel>> GetUserRatings()
    {
        var response = await _httpClient.GetAsync(_options.GetUsersRatingURL ?? "");
        return response.StatusCode == HttpStatusCode.OK
            ? await response.Content.ReadAsAsync<List<RatingViewModel>>()
            : response.StatusCode == HttpStatusCode.Unauthorized ? throw new CustomException() : new List<RatingViewModel>();
    }
}
