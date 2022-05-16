using System.Net;
using CRBClient.Helpers;
using CRBClient.Models;
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

    public async Task LoginUserAsync(UserViewModel user)
    {
        var response = await _httpClient.PostAsync("api/account/login", user);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var token = await response.Content.ReadAsStringAsync();
            _httpClient.SetTokenInHeader(token);
        }
        else
        {
            var errorMsg = await response.Content.ReadAsStringAsync();
            throw new CustomException(errorMsg);
        }
    }

    public string UpdateUserInfo()
    {
        throw new NotImplementedException();
    }
}
