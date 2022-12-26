using CRBClient.Dto;
using CRBClient.Models;

namespace CRBClient.Services.Interfaces;

public interface IRoomService
{
    Task<List<RoomViewModel>> GetRoomsAsync(bool isClosed, CancellationToken cancellationToken);

    Task<List<RoomViewModel>> GetFilteredCurrencyAsync(FilterDto filter, CancellationToken cancellationToken);
}
