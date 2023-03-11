using System.Net;
using CRBClient.Helpers;
using CRBClient.Models;
using System.Text.Json;
using Microsoft.Extensions.Options;
using CRBClient.Services.Interfaces;
using CRBClient.Dto;
using Uri = CRBClient.Helpers.Uri;

namespace CRBClient.Services;

public class RoomService : IRoomService
{
    private readonly ICRBServerHttpClient _httpClient;
    private readonly ILogger<RoomService> _logger;
    private readonly Uri _uri;

    public RoomService(ICRBServerHttpClient httpClient, ILogger<RoomService> logger, IOptions<Uri> uriOptions)
    {
        _httpClient = httpClient;
        _logger = logger;
        _uri = uriOptions.Value;
    }

    public async Task<List<RoomViewModel>> GetRoomsAsync(bool isClosed, CancellationToken cancellationToken)
    {
        var responseTask = await _httpClient.GetAsync(_uri.RoomsURL + $"?isClosed={isClosed}", cancellationToken);

        if (responseTask.StatusCode == HttpStatusCode.OK)
        {
            var result = await responseTask.Content.ReadAsStringAsync(cancellationToken);
            var rooms = JsonSerializer.Deserialize<IEnumerable<RoomViewModel>>(result);

            _logger.LogInformation("Rooms are loaded successfully.");
            return rooms == null ? throw new GeneralException("No rooms are available.") : rooms.ToList();
        }

        return responseTask.StatusCode == HttpStatusCode.Unauthorized
            ? throw new GeneralException("User unauthorized")
            : new List<RoomViewModel>();
    }

    public async Task<List<RoomViewModel>> GetFilteredCurrencyAsync(FilterDto filter, CancellationToken cancellationToken)
    {
        var responseTask = await _httpClient.PostAsync(_uri.RoomsFilterURL, filter, cancellationToken);

        if (responseTask.StatusCode == HttpStatusCode.OK)
        {
            var result = await responseTask.Content.ReadAsStringAsync(cancellationToken);
            var rooms = JsonSerializer.Deserialize<IEnumerable<RoomViewModel>>(result);

            _logger.LogInformation("Rooms are loaded successfully.");
            return rooms == null ? throw new GeneralException("No rooms are available.") : rooms.ToList();
        }

        return responseTask.StatusCode == HttpStatusCode.Unauthorized
            ? throw new GeneralException("User unauthorized")
            : new List<RoomViewModel>();
    }
}
