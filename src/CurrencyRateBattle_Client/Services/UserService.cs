using System.Net;
using CRBClient.Models;
using System.Net.Http.Formatting;
using CRBClient.Helpers;

namespace CRBClient.Services;

public class UserService : IUserService
{
    private const string ControllerPath = "api/account/";

    private readonly ILogger<UserService> _logger;

    private readonly HttpClient _httpClient;

    public UserService(ILogger<UserService> logger,
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }


    public async Task LoginAsync(UserViewModel user)
    {
        var response = await _httpClient.PostAsync(ControllerPath + "login", user, new JsonMediaTypeFormatter());

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var token = await response.Content.ReadAsStringAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("token", token);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new CustomException("You have not registered or entered incorrect data");
    }

    public Task RegistrationAsync(UserViewModel user)
    {
        throw new NotImplementedException();
    }
}
