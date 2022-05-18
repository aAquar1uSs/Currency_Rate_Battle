using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Managers.Interfaces;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
using CurrencyRateBattleServer.Tools;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Services;

public class RoomService : IRoomService
{
    private readonly ILogger<IAccountService> _logger;

    private readonly CurrencyRateBattleContext _DbContext;

    public RoomService(ILogger<AccountService> logger,
        CurrencyRateBattleContext DbContext)
    {
        _logger = logger;
        _DbContext = DbContext;
    }

    public Room CreateRoom(Room room)
    {
        var newRoom = _DbContext.Rooms.Add(room).Entity;
        _ = _DbContext.SaveChanges();
        if (newRoom == null)
            throw new CustomException($"{nameof(Room)} can not be created.");
        return newRoom;
    }
    public void UpdateRoom(Guid id, Room updatedRoom)
    {
        var room = _DbContext.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null)
            throw new CustomException($"{nameof(Room)} with Id={id} is not found.");
        _DbContext.Entry(room).Property(x => x.IsClosed).CurrentValue = updatedRoom.IsClosed;
        _DbContext.Entry(room).Property(x => x.Date).CurrentValue = updatedRoom.Date;
        //_DbContext.Entry(room).CurrentValues.SetValues(updatedRoom);
        _ = _DbContext.SaveChanges();
    }
    public async Task<List<Room>> GetRoomsAsync(bool isActive)
    {
        return isActive ? _DbContext.Rooms.Where(r => !r.IsClosed).ToList() : _DbContext.Rooms.ToList();
    }
    public Room? GetRoomById(Guid id)
    {
        return _DbContext.Rooms.FirstOrDefault(r => r.Id == id);
    }
}
