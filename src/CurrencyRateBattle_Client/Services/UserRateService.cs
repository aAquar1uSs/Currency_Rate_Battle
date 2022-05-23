using CRBClient.Helpers;
using CRBClient.Models;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;

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

    public string GetUserRates()
    {
        throw new NotImplementedException();
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
