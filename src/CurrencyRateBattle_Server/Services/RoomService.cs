using CurrencyRateBattleServer.Data;
using CurrencyRateBattleServer.Dto;
using CurrencyRateBattleServer.Helpers;
using CurrencyRateBattleServer.Models;
using CurrencyRateBattleServer.Services.Interfaces;
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

        await _semaphoreSlim.WaitAsync();
        try
        {
            var roomExists = await db.Rooms.AnyAsync(r => r.Id == id);
            if (!roomExists)
                throw new CustomException($"{nameof(Room)} with Id={id} is not found.");

            _ = db.Rooms.Update(updatedRoom);
            _ = await db.SaveChangesAsync();
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }
    }

    public async Task<List<RoomDto>> GetRoomsAsync(bool? isActive)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        List<RoomDto> roomDtoStorage = new();
        await _semaphoreSlim.WaitAsync();
        try
        {
            var result = from curr in db.Currencies
                join currState in db.CurrencyStates on curr.Id equals currState.CurrencyId
                join room in db.Rooms on currState.RoomId equals room.Id
                where room.IsClosed == isActive
                select new
                {
                    curr.CurrencyName,
                    room.Date,
                    room.IsClosed,
                    currState.CurrencyExchangeRate,
                    RateDate = currState.Date
                };

            foreach (var data in result)
            {
                roomDtoStorage.Add(new RoomDto
                {
                    CurrencyExchangeRate = Math.Round(data.CurrencyExchangeRate, 2),
                    СurrencyName = data.CurrencyName,
                    Date = data.Date,
                    IsClosed = data.IsClosed,
                    UpdateRateTime = data.RateDate
                });
            }
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return roomDtoStorage;
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

    public async Task<List<RoomDto>?> GetActiveRoomsWithFilterAsync(string currencyName)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var result = new List<RoomDto>();
        await _semaphoreSlim.WaitAsync();
        try
        {
            var curr = await db.Currencies
                .FirstOrDefaultAsync(curr => curr.CurrencyName == currencyName);

            if (curr is null)
                return null;

            result = GetActiveRoomsWithFilterByCurrencyId(db, curr.Id, currencyName);
        }
        finally
        {
            _ = _semaphoreSlim.Release();
        }

        return result;
    }

    private List<RoomDto> GetActiveRoomsWithFilterByCurrencyId(
        CurrencyRateBattleContext db,
        Guid currId,
        string currName)
    {
        var result = new List<RoomDto>();

        var rooms =
                from currencyState in db.CurrencyStates
                join room in db.Rooms
                on currencyState.RoomId equals room.Id
                select new
                {
                    room.Date,
                    currencyState.CurrencyExchangeRate,
                    currencyState.CurrencyId,
                    room.IsClosed
                };

        var filteredRooms = rooms.Where(r => r.CurrencyId == currId && r.IsClosed == false);

        foreach (var room in filteredRooms)
        {
            result.Add(
                new RoomDto()
                {
                    CurrencyName = currName,
                    CurrencyRate = room.CurrencyExchangeRate,
                    Time = room.Date
                });
        }

        return result;
    }
}
