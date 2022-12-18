using System.Globalization;
using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Dal.Services.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.Dal.Repositories;

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

    public async Task UpdateAsync(RoomDal updatedRoomDal, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(UpdateAsync)} was caused");

        _ = _dbContext.Rooms.Update(updatedRoomDal);
        _ = await _dbContext.SaveChangesAsync(cancellationToken);
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
            await UpdateAsync(roomDal.Id, roomDal);
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
                await UpdateAsync(roomDal, CancellationToken.None);
        }
    }

    public async Task<RoomDal?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(FindAsync)} was caused");
        return await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);;
    }

    public async Task DeleteAsync(RoomId roomId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(DeleteAsync)} was caused");
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == roomId.Id, cancellationToken);

            if (room is null)
                return;

            _ = _dbContext.Remove(room);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
