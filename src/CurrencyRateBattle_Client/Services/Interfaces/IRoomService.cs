using CRBClient.Models;

namespace CRBClient.Services.Interfaces;

public interface IRoomService
{
    public Task<List<RoomViewModel>> GetRooms(bool isClosed);
}
