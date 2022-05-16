using CRBClient.Models;

namespace CRBClient.Services;
public interface ICRBServer
{
    public Task<IEnumerable<RoomViewModel>> GetRooms();
}
