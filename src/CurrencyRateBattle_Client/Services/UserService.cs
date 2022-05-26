using System.Globalization;
using System.Net;
using CRBClient.Helpers;
using CRBClient.Models;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;

namespace CRBClient.Services;

public class UserService : IUserService
{
    private readonly ICRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<UserService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession Session => _httpContextAccessor.HttpContext.Session;

    public UserService(ICRBServerHttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        IOptions<WebServerOptions> options,
        ILogger<UserService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserInfo()
    {
        throw new NotImplementedException();
    }

    public async Task RegisterUserAsync(UserViewModel user)
    {
        var response = await _httpClient.PostAsync(_options.RegistrationAccURL ?? "", user);

        if (!user.Password.Equals(user.ConfirmPassword, StringComparison.Ordinal))
        {
            throw new CustomException("Password is not confirmed.");
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var token = await response.Content.ReadAsStringAsync();
            Session.SetString("token", token);
        }
        else
        {
            var errorText = await response.Content.ReadAsStringAsync();
            throw new CustomException(errorText);
        }
    }

    public async Task LoginUserAsync(UserViewModel user)
    {
        var response = await _httpClient.PostAsync(_options.LoginAccURL ?? "", user);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var token = await response.Content.ReadAsStringAsync();

            Session.SetString("token", token);
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

    public async Task<AccountInfoViewModel> GetAccountInfoAsync()
    {
        var response = await _httpClient.GetAsync(_options.UserProfileURL ?? "");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsAsync<AccountInfoViewModel>();
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new CustomException();
        }

        return new AccountInfoViewModel();
    }

    public async Task<List<AccountHistoryViewModel>> GetAccountHistoryAsync()
    {
        var response = await _httpClient.GetAsync("api/history");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsAsync<List<AccountHistoryViewModel>>();
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new CustomException();
        }

        return new List<AccountHistoryViewModel>();
    }

    public async Task<string> GetUserBalanceAsync()
    {
        var balance = string.Empty;
        var response = await _httpClient.GetAsync(_options.GetBalanceURL ?? "");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (decimal.TryParse(response.Content.ReadAsStringAsync().Result.Replace('.',','), out var bal))
            {
                balance = "BALANCE: " + bal.ToString("C", new CultureInfo("uk-UA"));
            }
        }

        return response.StatusCode == HttpStatusCode.Unauthorized ? throw new CustomException() : balance;
    }

    public void Logout()
    {
        Session.Remove("token");
        _httpClient.ClearHeader();
    }
}
