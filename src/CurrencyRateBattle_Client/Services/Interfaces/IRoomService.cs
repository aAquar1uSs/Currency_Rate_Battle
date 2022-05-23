using CRBClient.Models;

namespace CRBClient.Services.Interfaces;

public interface IRoomService
{
    public Task<List<RoomViewModel>> GetRoomsAsync(bool isClosed);

    public Task<List<RoomViewModel>> GetFilteredCurrencyAsync(string currencyName);
}
