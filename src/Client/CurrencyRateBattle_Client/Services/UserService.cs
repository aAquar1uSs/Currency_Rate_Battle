using System.Globalization;
using System.Net;
using System.Text.Json;
using CRBClient.Dto;
using CRBClient.Helpers;
using CRBClient.Models;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;
using Uri = CRBClient.Helpers.Uri;

namespace CRBClient.Services;

public class UserService : IUserService
{
    private readonly ICRBServerHttpClient _httpClient;
    private readonly Uri _uri;
    private readonly ILogger<UserService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession? Session => _httpContextAccessor.HttpContext?.Session;

    public UserService(ICRBServerHttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        IOptions<Uri> options,
        ILogger<UserService> logger)
    {
        _httpClient = httpClient;
        _uri = options.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task RegisterUserAsync(UserViewModel user, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync(_uri.RegistrationAccURL ?? "", user, cancellationToken);

        if (user.Password != user.ConfirmPassword)
        {
            _logger.LogInformation("Password is not confirmed");
            throw new GeneralException("Password is not confirmed.");
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User was successfully registered");
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var token = JsonSerializer.Deserialize<TokenDto>(content);
            if (Session is not null)
                Session.SetString("token", token?.Token);
        }
        else
        {
            _logger.LogInformation("User was not successfully registered");
            var errorText = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new GeneralException(errorText);
        }
    }

    public async Task LoginUserAsync(UserViewModel user, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync(_uri.LoginAccURL, user, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User was successfully login");
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var token = JsonSerializer.Deserialize<TokenDto>(content);
            if (Session is not null)
                Session.SetString("token", token?.Token);
        }
        else
        {
            _logger.LogInformation("User was not successfully login");
            var errorMsg = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new GeneralException(errorMsg);
        }
    }

    public async Task<AccountInfoViewModel> GetAccountInfoAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_uri.UserProfileURL, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("Account info are loaded successfully");
            return await response.Content.ReadAsAsync<AccountInfoViewModel>(cancellationToken);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("Account info are not loaded, user is unauthorized");
            throw new GeneralException();
        }
        return new AccountInfoViewModel();
    }

    public async Task<List<AccountHistoryViewModel>> GetAccountHistoryAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_uri.AccountHistoryURL, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("Account history are loaded successfully");
            return await response.Content.ReadAsAsync<List<AccountHistoryViewModel>>(cancellationToken);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("Account history are not loaded, user is unauthorized");
            throw new GeneralException();
        }
        return new List<AccountHistoryViewModel>();
    }

    public async Task<string> GetUserBalanceAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_uri.GetBalanceURL, cancellationToken);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User balance are loaded successfully");
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var money = JsonSerializer.Deserialize<AmountDto>(content);
            
            return "BALANCE: " + money?.Amount.ToString("C", new CultureInfo("uk-UA"));
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("User balance are not loaded, user is unauthorized");
            throw new GeneralException();
        }
        return string.Empty;;
    }

    public async Task<decimal> GetUserBalanceDecimalAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(_uri.GetBalanceURL, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("User balance are loaded successfully");
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var money = JsonSerializer.Deserialize<AmountDto>(content);
            return money.Amount;
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("User balance are not loaded, user is unauthorized");
            throw new GeneralException();
        }
        return decimal.Zero;
    }

    public void Logout()
    {
        if (Session is not null)
            Session.Remove("token");
        _httpClient.ClearHeader();
    }
}
