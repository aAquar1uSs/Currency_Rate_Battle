using CRBClient.Helpers;
using CRBClient.Models;
using CRBClient.Services.Interfaces;
using System.Net;
using Microsoft.Extensions.Options;
using Uri = CRBClient.Helpers.Uri;

namespace CRBClient.Services;

public class UserRateService : IUserRateService
{
    private readonly ICRBServerHttpClient _httpClient;
    private readonly ILogger<UserRateService> _logger;
    private readonly Uri _uri;

    public UserRateService(ICRBServerHttpClient httpClient, ILogger<UserRateService> logger, IOptions<Uri> uriOptions)
    {
        _httpClient = httpClient;
        _logger = logger;
        _uri = uriOptions.Value;
    }

    public async Task<List<BetViewModel>> GetUserRates(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_uri.GetUserBetsURL, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User rates are loaded successfully");
            return await response.Content.ReadAsAsync<List<BetViewModel>>(cancellationToken);
        }
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("User rates not loaded, user is unauthorized");
            throw new GeneralException();
        }
        return new List<BetViewModel>();
    }


    public async Task InsertUserRateAsync(RateViewModel rateViewModel,CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync(_uri.MakeBetURL, rateViewModel, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User rate is inserted.");
            return;
        }

        var errorMsg = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogError("Rate Insertion: {ErrorMsg}", errorMsg);
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
