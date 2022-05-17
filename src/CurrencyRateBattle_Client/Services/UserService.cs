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

    public async Task RegisterUserAsync(UserViewModel user)
    {
        var response = await _httpClient.PostAsync("api/account/registration", user);

        if (!user.Password.Equals(user.ConfirmPassword, StringComparison.Ordinal))
        {
            throw new CustomException("Password is not confirmed.");
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var token = await response.Content.ReadAsStringAsync();
            _httpClient.SetTokenInHeader(token);
        }
        else
        {
            var errorText = await response.Content.ReadAsStringAsync();
            throw new CustomException(errorText);
        }
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
            throw new CustomException("This user does not exist. Please check the entered data and try again.");
        }
    }

    public string UpdateUserInfo()
    {
        throw new NotImplementedException();
    }
}
