using CRBClient.Dto;
using CRBClient.Models;

namespace CRBClient.Services.Interfaces;

public interface IRoomService
{
    public Task<List<RoomViewModel>> GetRoomsAsync(bool isClosed);

    public Task<List<RoomViewModel>> GetFilteredCurrencyAsync(FilterDto filter);
}
