using CurrencyRateBattleServer.Dal.Repositories.Interfaces;
using CurrencyRateBattleServer.Domain.Entities;
using CurrencyRateBattleServer.Domain.Entities.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRateBattleServer.ApplicationServices.Handlers.RoomHandlers.GenerateRoomHandler;

public class GenerateRoomHandler : IRequestHandler<GenerateRoomCommand>
{
    private readonly ILogger<GenerateRoomHandler> _logger;
    private readonly IRoomRepository _roomRepository;
    private readonly ICurrencyQueryRepository _currencyQueryRepository;

    public GenerateRoomHandler(ILogger<GenerateRoomHandler> logger, IRoomRepository roomRepository, ICurrencyQueryRepository currencyQueryRepository)
    {
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _currencyQueryRepository = currencyQueryRepository ?? throw new ArgumentNullException(nameof(currencyQueryRepository));
    }

    //Add settings to count of rooms can be generated
    public async Task<Unit> Handle(GenerateRoomCommand request, CancellationToken cancellationToken)
    {
        var currencies = await _currencyQueryRepository.GetAllIds(cancellationToken);

        foreach (var currency in currencies)
        {
            var roomId = RoomId.GenerateId();
            var room = Room.Create(roomId.Id, DateTime.UtcNow.AddDays(1), false, 0, currency);

            await _roomRepository.CreateAsync(room, cancellationToken);
        }
        
        return Unit.Value;
    }
}
