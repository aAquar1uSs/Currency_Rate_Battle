using System.Net;
using CRBClient.Helpers;
using CRBClient.Models;
using System.Text.Json;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;
using CRBClient.Dto;

namespace CRBClient.Services;

public class RoomService : IRoomService
{
    private readonly ICRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<RoomService> _logger;

    public RoomService(ICRBServerHttpClient httpClient,
        IOptions<WebServerOptions> options, ILogger<RoomService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<List<RoomViewModel>> GetRoomsAsync(bool isClosed)
    {
        var responseTask = await _httpClient.GetAsync(_options.RoomsURL + $"/{isClosed}");

        if (responseTask.StatusCode == HttpStatusCode.OK)
        {
            var result = await responseTask.Content.ReadAsStringAsync();
            var rooms = JsonSerializer.Deserialize<IEnumerable<RoomViewModel>>(result);

            _logger.LogInformation("Rooms are loaded successfully.");
            return rooms == null ? throw new CustomException("No rooms are available.") : rooms.ToList();
        }

        return responseTask.StatusCode == HttpStatusCode.Unauthorized
            ? throw new CustomException("User unauthorized")
            : new List<RoomViewModel>();
    }

    public async Task<List<RoomViewModel>> GetFilteredCurrencyAsync(FilterDto filter)
    {
        var responseTask = await _httpClient.PostAsync(_options.FilterURL ?? "", filter);

        if (responseTask.StatusCode == HttpStatusCode.OK)
        {
            var result = await responseTask.Content.ReadAsStringAsync();
            var rooms = JsonSerializer.Deserialize<IEnumerable<RoomViewModel>>(result);

            _logger.LogInformation("Rooms are loaded successfully.");
            return rooms == null ? throw new CustomException("No rooms are available.") : rooms.ToList();
        }

        return responseTask.StatusCode == HttpStatusCode.Unauthorized
            ? throw new CustomException("User unauthorized")
            : new List<RoomViewModel>();
    }
}
