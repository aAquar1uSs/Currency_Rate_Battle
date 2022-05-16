using CRBClient.Helpers;
using CRBClient.Models;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace CRBClient.Services;

public class CRBServer : ICRBServer
{
    private readonly CRBServerHttpClient _httpClient;
    private readonly WebServerOptions _options;
    private readonly ILogger<CRBServer> _logger;

    public CRBServer(CRBServerHttpClient httpClient,
           IOptions<WebServerOptions> options, ILogger<CRBServer> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }
    public async Task<IEnumerable<RoomViewModel>> GetRooms()
    {
        var rooms = Enumerable.Empty<RoomViewModel>();
        //HTTP GET
        var responseTask = _httpClient.GetAPIResults(_options.RoomsURL);
        responseTask.Wait();

        var result = responseTask.Result;
        rooms = JsonSerializer.Deserialize<IEnumerable<RoomViewModel>>(result);

        // rooms = Enumerable.Empty<RoomViewModel>();

        //_logger.LogInformation("Rooms are not loaded. Something bad has happened :(");
        _logger.LogInformation("Rooms are loaded successfully.");
        return rooms;
    }
}
