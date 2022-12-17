using System.Globalization;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Services;

public class RoomRepository : IRoomRepository
{
    private readonly ILogger<RoomRepository> _logger;

    private readonly IRateCalculationRepository _rateCalculationRepository;

    private readonly CurrencyRateBattleContext _dbContext;

    public RoomRepository(ILogger<RoomRepository> logger, IRateCalculationRepository rateCalculationRepository,
        CurrencyRateBattleContext dbContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rateCalculationRepository = rateCalculationRepository ?? throw new ArgumentNullException(nameof(rateCalculationRepository));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task GenerateRoomsByCurrencyCountAsync()
    {
        _logger.LogInformation($"{nameof(GenerateRoomsByCurrencyCountAsync)} was caused");

        foreach (var curr in _dbContext.Currencies)
        {
            _ = await _dbContext.CurrencyStates.AddAsync(await CreateRoomWithCurrencyStateAsync(curr));
        }

        _ = await _dbContext.SaveChangesAsync();
    }


public Task<CurrencyState> CreateRoomWithCurrencyStateAsync(CurrencyDal curr)
    {
        _logger.LogInformation($"{nameof(CreateRoomWithCurrencyStateAsync)} was caused");
        var currentDate = DateTime.ParseExact(
            DateTime.UtcNow.ToString("MM.dd.yyyy HH:00:00", CultureInfo.InvariantCulture),
            "MM.dd.yyyy HH:mm:ss", null);

        return Task.FromResult(new CurrencyState
        {
            Date = currentDate,
            CurrencyExchangeRate = 0,
            Currency = curr,
            CurrencyId = curr.Id,
            Room = new RoomDal { Date = currentDate.AddDays(1), IsClosed = false }
        });
    }

    public async Task UpdateRoomAsync(Guid id, RoomDal updatedRoomDal)
    {
        _logger.LogInformation($"{nameof(UpdateRoomAsync)} was caused");
        
            var roomExists = await _dbContext.Rooms.AnyAsync(r => r.Id == id);
            if (!roomExists)
                throw new GeneralException($"{nameof(RoomDal)} with Id={id} is not found.");

            _ = _dbContext.Rooms.Update(updatedRoomDal);
            _ = await _dbContext.SaveChangesAsync();
    }

    public async Task CheckRoomsStateAsync()
    {
        _logger.LogInformation($"{nameof(CheckRoomsStateAsync)} was caused");

        foreach (var room in _dbContext.Rooms)
        {
            await RoomClosureCheckAsync(room);
            await CalculateRatesIfRoomClosed(room);
        }
    }

    private async Task RoomClosureCheckAsync(RoomDal roomDal)
    {
        _logger.LogInformation($"{nameof(RoomClosureCheckAsync)} was caused");
        if ((roomDal.Date.Date == DateTime.Today
             && roomDal.Date.Hour == DateTime.UtcNow.AddHours(1).Hour)
            || ((roomDal.Date.Date == DateTime.Today.AddDays(1))
            && roomDal.Date.Hour == 0 && DateTime.UtcNow.Hour == 23)
            || DateTime.UtcNow > roomDal.Date)
        {
            roomDal.IsClosed = true;
            await UpdateRoomAsync(roomDal.Id, roomDal);
        }
    }

    private async Task CalculateRatesIfRoomClosed(RoomDal roomDal)
    {
        _logger.LogInformation($"{nameof(CalculateRatesIfRoomClosed)} was caused");
        if ((roomDal.Date.Date == DateTime.Today
             && roomDal.Date.Hour == DateTime.UtcNow.Hour
             && roomDal.IsClosed)
            || (DateTime.UtcNow > roomDal.Date
                && roomDal.IsClosed))
        {
            await _rateCalculationRepository.StartRateCalculationByRoomIdAsync(roomDal.Id);
                await UpdateRoomAsync(roomDal.Id, roomDal);
        }
    }

    public Task<Room[]> GetRoomsAsync(bool? isClosed)
    {
        _logger.LogInformation($"{nameof(GetRoomsAsync)} was caused");

        List<RoomDto> roomDtoStorage = new();
        var result = from curr in _dbContext.Currencies
                     join currState in _dbContext.CurrencyStates on curr.Id equals currState.CurrencyId
                     join room in db.Rooms on currState.RoomId equals room.Id
                     where room.IsClosed == isClosed
                     select new
                     {
                         room.Id,
                         curr.CurrencyName,
                         room.Date,
                         room.IsClosed,
                         currState.CurrencyExchangeRate,
                         RateDate = currState.Date,
                         RateCount = db.Rates.Count(r => r.RoomId == room.Id)
                     };

        foreach (var data in result)
        {
            roomDtoStorage.Add(new RoomDto
            {
                Id = data.Id,
                CurrencyExchangeRate = Math.Round(data.CurrencyExchangeRate, 2),
                СurrencyName = data.CurrencyName,
                Date = data.Date,
                IsClosed = data.IsClosed,
                UpdateRateTime = data.RateDate,
                CountRates = data.RateCount
            });
        }

        return Task.FromResult(roomDtoStorage);
    }

    public async Task<RoomDal?> GetRoomByIdAsync(Guid id)
    {
        _logger.LogInformation($"{nameof(GetRoomByIdAsync)} was caused");
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CurrencyRateBattleContext>();

        var result = await db.Rooms.FirstOrDefaultAsync(r => r.Id == id);

        return result;
    }

    public Task<List<RoomDto>?> GetActiveRoomsWithFilterAsync(Filter filter)
    {
        _logger.LogInformation($"{nameof(GetActiveRoomsWithFilterAsync)} was caused");

        var result = new List<RoomDto>();

        var filteredRooms =
            from currencyState in _dbContext.CurrencyStates
            join room in _dbContext.Rooms on currencyState.RoomId equals room.Id
            join curr in _dbContext.Currencies on currencyState.CurrencyId equals curr.Id
            where room.IsClosed == false
            select new
            {
                room.Id,
                room.Date,
                currencyState.CurrencyExchangeRate,
                currencyState.CurrencyId,
                room.IsClosed,
                curr.CurrencyName,
                RateUpdateDate = currencyState.Date,
                RateCount = db.Rates.Count(r => r.RoomId == room.Id)
            };

        if (!string.IsNullOrWhiteSpace(filter.CurrencyName))
            filteredRooms =
                filteredRooms.Where(room => room.CurrencyName == filter.CurrencyName.ToUpperInvariant());
        if (filter.DateTryParse(filter.StartDate, out var startDate))
            filteredRooms = filteredRooms.Where(room => room.Date >= startDate);
        if (filter.DateTryParse(filter.EndDate, out var endDate))
            filteredRooms = filteredRooms.Where(room => room.Date <= endDate);

        foreach (var room in filteredRooms)
        {
            result.Add(
                new RoomDto
                {
                    Id = room.Id,
                    CurrencyExchangeRate = room.CurrencyExchangeRate,
                    Date = room.Date,
                    СurrencyName = room.CurrencyName,
                    UpdateRateTime = room.RateUpdateDate,
                    IsClosed = room.IsClosed,
                    CountRates = room.RateCount
                });
        }

        return Task.FromResult(result);
    }

    public async Task DeleteRoomByIdAsync(Guid roomId)
    {
        _logger.LogInformation($"{nameof(DeleteRoomByIdAsync)} was caused");
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);

            if (room is null)
                return;

            _ = _dbContext.Remove(room);
            _ = await _dbContext.SaveChangesAsync();
    }
}
