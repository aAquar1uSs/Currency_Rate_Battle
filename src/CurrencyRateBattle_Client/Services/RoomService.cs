using System.Net;
using CRBClient.Helpers;
using CRBClient.Models;
using System.Text.Json;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;

namespace CRBClient.Services;

public class RoomService : IRoomService
{
    private readonly CRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<RoomService> _logger;

    public RoomService(CRBServerHttpClient httpClient,
        IOptions<WebServerOptions> options, ILogger<RoomService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<List<RoomViewModel>> GetRooms(bool isClosed)
    {
        var rooms = Enumerable.Empty<RoomViewModel>();

        var responseTask = await _httpClient.GetAsync(_options.RoomsURL + $"/{isClosed}");

        if (responseTask.StatusCode == HttpStatusCode.OK)
        {
            var result = await responseTask.Content.ReadAsStringAsync();
            rooms = JsonSerializer.Deserialize<IEnumerable<RoomViewModel>>(result);

            _logger.LogInformation("Rooms are loaded successfully.");
            return rooms.ToList();
        }

        if (responseTask.StatusCode == HttpStatusCode.Unauthorized)
            throw new CustomException("User unauthorized");

        return new List<RoomViewModel>();
    }
}
