using CRBClient.Helpers;
using CRBClient.Models;
using System.Text.Json;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;

namespace CRBClient.Services;

public class UserService : IUserService
{
    private readonly CRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<UserService> _logger;

    public UserService(CRBServerHttpClient httpClient,
           IOptions<WebServerOptions> options, ILogger<UserService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public string GetUserInfo()
    {
        throw new NotImplementedException();
    }

    public string RegisterUser()
    {
        throw new NotImplementedException();
    }

    public string UpdateUserInfo()
    {
        throw new NotImplementedException();
    }
}
