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
        return _DbContext.Rooms.Add(room).Entity;

    }
    public void UpdateRoom(Room room)
    {
        _DbContext.Update(room);
    }
    public List<Room> GetRooms()
    {
        return _DbContext.Rooms.ToList();
    }
    public List<Room> GetActiveRooms()
    {
        return _DbContext.Rooms.Where(r => !r.IsClosed).ToList();
    }

}
