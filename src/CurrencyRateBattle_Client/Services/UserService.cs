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
    private ISession? Session => _httpContextAccessor.HttpContext?.Session;

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

    public async Task RegisterUserAsync(UserViewModel user)
    {
        var response = await _httpClient.PostAsync(_options.RegistrationAccURL ?? "", user);

        if (!user.Password.Equals(user.ConfirmPassword, StringComparison.Ordinal))
        {
            throw new GeneralException("Password is not confirmed.");
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var token = await response.Content.ReadAsStringAsync();
            if (Session is not null) 
                Session.SetString("token", token);
        }
        else
        {
            var errorText = await response.Content.ReadAsStringAsync();
            throw new GeneralException(errorText);
        }
    }

    public async Task LoginUserAsync(UserViewModel user)
    {
        var response = await _httpClient.PostAsync(_options.LoginAccURL ?? "", user);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var token = await response.Content.ReadAsStringAsync();

            if (Session is not null)
                Session.SetString("token", token);
        }
        else
        {
            var errorMsg = await response.Content.ReadAsStringAsync();
            throw new GeneralException(errorMsg);
        }
    }

    public async Task<AccountInfoViewModel> GetAccountInfoAsync()
    {
        var response = await _httpClient.GetAsync(_options.UserProfileURL ?? "");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsAsync<AccountInfoViewModel>();
        }

        return response.StatusCode == HttpStatusCode.Unauthorized ? throw new GeneralException() : new AccountInfoViewModel();
    }

    public async Task<List<AccountHistoryViewModel>> GetAccountHistoryAsync()
    {
        var response = await _httpClient.GetAsync("api/history");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadAsAsync<List<AccountHistoryViewModel>>();
        }

        return response.StatusCode == HttpStatusCode.Unauthorized ? throw new GeneralException() : new List<AccountHistoryViewModel>();
    }

    public async Task<string> GetUserBalanceAsync()
    {
        var balance = string.Empty;
        var response = await _httpClient.GetAsync(_options.GetBalanceURL ?? "");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (decimal.TryParse(await response.Content.ReadAsStringAsync(),
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var bal))
            {
                balance = "BALANCE: " + bal.ToString("C", new CultureInfo("uk-UA"));
            }
        }

        return response.StatusCode == HttpStatusCode.Unauthorized ? throw new GeneralException() : balance;
    }

    public async Task<decimal> GetUserBalanceDecimalAsync()
    {
        var balance = 0M;
        var response = await _httpClient.GetAsync(_options.GetBalanceURL ?? "");

        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (decimal.TryParse(await response.Content.ReadAsStringAsync(),
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var bal))
            {
                balance = bal;
            }
        }

        return response.StatusCode == HttpStatusCode.Unauthorized ? throw new GeneralException() : balance;
    }

    public void Logout()
    {
        if (Session is not null)
            Session.Remove("token");
        _httpClient.ClearHeader();
    }
}
