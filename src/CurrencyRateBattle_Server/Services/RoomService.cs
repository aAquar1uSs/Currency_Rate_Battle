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

    private readonly IServiceScopeFactory _scopeFactory;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public RoomService(ILogger<AccountService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task<Room> CreateRoomAsync(Room room)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();
        Room newRoom;
        await _semaphoreSlim.WaitAsync();
        try
        {
            newRoom = db.Rooms.Add(room).Entity;
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return newRoom ?? throw new CustomException($"{nameof(Room)} can not be created.");
    }
    public async void UpdateRoomAsync(Guid id, Room updatedRoom)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        Room room;

        await _semaphoreSlim.WaitAsync();
        try
        {
            room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                throw new CustomException($"{nameof(Room)} with Id={id} is not found.");
            db.Entry(room).Property(x => x.IsClosed).CurrentValue = updatedRoom.IsClosed;
            db.Entry(room).Property(x => x.Date).CurrentValue = updatedRoom.Date;
            //db.Entry(room).CurrentValues.SetValues(updatedRoom);
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

    }
    public async Task<List<Room>> GetRoomsAsync(bool? isActive)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        List<Room> result;
        await _semaphoreSlim.WaitAsync();
        try
        {
            result = isActive == true
                ? await db.Rooms.Where(r => !r.IsClosed).ToListAsync()
                : isActive == false ? await db.Rooms.Where(r => r.IsClosed).ToListAsync() : await db.Rooms.ToListAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return result;
    }
    public async Task<Room?> GetRoomByIdAsync(Guid id)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        Room result;
        await _semaphoreSlim.WaitAsync();
        try
        {
            result = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return result;
    }
}
