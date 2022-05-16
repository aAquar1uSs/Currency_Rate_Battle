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
    public async Task<IEnumerable<RoomViewModel>> GetRooms()
    {
        var rooms = Enumerable.Empty<RoomViewModel>();
        //HTTP GET
        var responseTask = _httpClient.GetAPIResultsAsync(_options.RoomsURL);
        responseTask.Wait();

        var result = responseTask.Result;
        rooms = JsonSerializer.Deserialize<IEnumerable<RoomViewModel>>(result);

        // rooms = Enumerable.Empty<RoomViewModel>();

        //_logger.LogInformation("Rooms are not loaded. Something bad has happened :(");
        _logger.LogInformation("Rooms are loaded successfully.");
        return rooms;
    }
}
